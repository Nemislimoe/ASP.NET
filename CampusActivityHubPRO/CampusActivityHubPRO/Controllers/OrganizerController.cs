using CampusActivityHubPRO.Data;
using CampusActivityHubPRO.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusActivityHubPRO.Controllers
{
    [Authorize(Roles = "Organizer")]
    public class OrganizerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public OrganizerController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: /Organizer/MyEvents
        [HttpGet]
        public async Task<IActionResult> MyEvents()
        {
            var userId = _userManager.GetUserId(User);
            var events = await _db.Events
                .Where(e => e.OrganizerId == userId)
                .Include(e => e.Registrations)
                .Include(e => e.Category)
                .Include(e => e.EventTags!)
                    .ThenInclude(et => et.Tag)
                .OrderByDescending(e => e.StartAt)
                .ToListAsync();

            return View(events);
        }

        // GET: /Organizer/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            return View("Create", new Event { StartAt = DateTime.UtcNow.AddDays(1), EndAt = DateTime.UtcNow.AddDays(1).AddHours(2) });
        }

        // POST: /Organizer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model, IFormFile? poster, int[]? selectedTags)
        {
            // Basic model validation
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError(nameof(model.Title), "Назва події обов'язкова.");
            }

            // Custom validators
            if (model.StartAt <= DateTime.UtcNow)
            {
                ModelState.AddModelError(nameof(model.StartAt), "Дата події має бути в майбутньому.");
            }
            if (model.StartAt.Hour < 7 || model.StartAt.Hour >= 23)
            {
                ModelState.AddModelError(nameof(model.StartAt), "Подія має починатися між 07:00 та 23:00.");
            }
            if (model.Capacity <= 0)
            {
                ModelState.AddModelError(nameof(model.Capacity), "Ліміт учасників має бути більшим за 0.");
            }

            // Poster validation (if provided)
            if (poster != null)
            {
                var ext = Path.GetExtension(poster.FileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    ModelState.AddModelError("poster", "Підтримуються лише JPEG/PNG формати для постера.");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Tags = await _db.Tags.ToListAsync();
                return View("Create", model);
            }

            // Handle poster file (filesystem)
            if (poster != null)
            {
                var ext = Path.GetExtension(poster.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{ext}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using var stream = System.IO.File.Create(path);
                await poster.CopyToAsync(stream);
                model.PosterPath = $"/uploads/{fileName}";
            }

            // Set organize
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(); // Return an appropriate response if the user is not authenticated.
            }
            model.OrganizerId = userId;
            // Add event
            _db.Events.Add(model);
            await _db.SaveChangesAsync();

            // Tags
            if (selectedTags != null && selectedTags.Length > 0)
            {
                foreach (var t in selectedTags.Distinct())
                {
                    _db.EventTags.Add(new EventTag { EventId = model.Id, TagId = t });
                }
                await _db.SaveChangesAsync();
            }

            // Notify organizer (self) by email (fake sender logs or uses Mailtrap if configured)
            var organizer = await _db.Users.FindAsync(userId);
            if (organizer != null)
            {
                await _emailSender.SendEmailAsync(organizer.Email!, "Подію створено", $"Ваша подія '{model.Title}' успішно створена та опублікована.");
            }

            TempData["CreateSuccess"] = "Подію створено.";
            return RedirectToAction("Details", "Events", new { id = model.Id });
        }

        // GET: /Events/Edit/{id}  (Organizer edits own event)
        [HttpGet]
        [Route("Events/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var ev = await _db.Events
                .Include(e => e.EventTags)
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizerId == userId);

            if (ev == null) return NotFound();

            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            return View("Edit", ev);
        }

        // POST: /Events/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Events/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, Event model, IFormFile? poster, int[]? selectedTags)
        {
            var formDump = string.Join("; ", Request.Form.Select(kv => $"{kv.Key}={kv.Value}"));
            Console.WriteLine("FORM: " + formDump);
            TempData["DebugForm"] = formDump;
            if (id != model.Id)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);
            var ev = await _db.Events
                .Include(e => e.Registrations)
                .Include(e => e.EventTags)
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizerId == userId);

            if (ev == null) return NotFound();

            // Custom validation: start date in future and between 07:00 and 23:00
            if (model.StartAt <= DateTime.UtcNow)
            {
                ModelState.AddModelError(nameof(model.StartAt), "Дата події має бути в майбутньому.");
            }
            if (model.StartAt.Hour < 7 || model.StartAt.Hour >= 23)
            {
                ModelState.AddModelError(nameof(model.StartAt), "Подія має починатися між 07:00 та 23:00.");
            }

            // Capacity cannot be less than already registered
            var registeredCount = ev.Registrations?.Count ?? 0;
            if (model.Capacity < registeredCount)
            {
                ModelState.AddModelError(nameof(model.Capacity), $"Ліміт учасників не може бути меншим за вже зареєстрованих ({registeredCount}).");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Tags = await _db.Tags.ToListAsync();
                return View("Edit", model);
            }

            // Update basic fields
            ev.Title = model.Title;
            ev.Description = model.Description;
            ev.StartAt = model.StartAt;
            ev.EndAt = model.EndAt;
            ev.Capacity = model.Capacity;
            ev.CategoryId = model.CategoryId;

            // Poster handling (filesystem)
            if (poster != null)
            {
                var ext = Path.GetExtension(poster.FileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    ModelState.AddModelError("poster", "Підтримуються лише JPEG/PNG.");
                    ViewBag.Categories = await _db.Categories.ToListAsync();
                    ViewBag.Tags = await _db.Tags.ToListAsync();
                    return View("Edit", model);
                }

                var fileName = $"{Guid.NewGuid()}{ext}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using var stream = System.IO.File.Create(path);
                await poster.CopyToAsync(stream);

                // Optionally delete old poster file
                if (!string.IsNullOrEmpty(ev.PosterPath))
                {
                    try
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ev.PosterPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    catch { /* ignore deletion errors */ }
                }

                ev.PosterPath = $"/uploads/{fileName}";
            }

            // Update tags: remove existing and add selected
            var existingTags = ev.EventTags?.ToList() ?? new List<EventTag>();
            foreach (var et in existingTags)
            {
                _db.EventTags.Remove(et);
            }

            if (selectedTags != null && selectedTags.Length > 0)
            {
                foreach (var tId in selectedTags.Distinct())
                {
                    _db.EventTags.Add(new EventTag { EventId = ev.Id, TagId = tId });
                }
            }

            await _db.SaveChangesAsync();

            TempData["EditSuccess"] = "Подію успішно оновлено.";
            return RedirectToAction("MyEvents");
        }

        // POST: /Organizer/Delete/{id}  (soft delete by organizer)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Organizer/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var ev = await _db.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizerId == userId);

            if (ev == null) return NotFound();

            // Prevent organizer from deleting events that already have registrations
            var regCount = ev.Registrations?.Count ?? 0;
            if (regCount > 0)
            {
                TempData["DeleteError"] = $"Неможливо видалити подію — вже є {regCount} реєстрацій.";
                return RedirectToAction("MyEvents");
            }

            ev.IsDeleted = true;
            await _db.SaveChangesAsync();

            TempData["DeleteSuccess"] = "Подію видалено (soft delete).";
            return RedirectToAction("MyEvents");
        }

        // Optional: AJAX endpoint to fetch organizer events (attribute routing example)
        [HttpGet]
        [Route("api/organizer/{organizerId}/events")]
        public async Task<IActionResult> ApiGetOrganizerEvents(string organizerId)
        {
            var events = await _db.Events
                .Where(e => e.OrganizerId == organizerId)
                .Select(e => new
                {
                    e.Id,
                    e.Title,
                    e.StartAt,
                    e.EndAt,
                    e.Capacity,
                    Registered = e.Registrations!.Count
                })
                .ToListAsync();

            return Json(events);
        }
    }
}
