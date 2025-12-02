using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CampusActivityHub.Data;
using CampusActivityHub.Models;
using CampusActivityHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusActivityHub.Controllers
{
    [Authorize(Roles = "Organizer")]
    [Route("organizer")]
    public class OrganizerController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender;

        public OrganizerController(AppDbContext db, IWebHostEnvironment env, IEmailSender emailSender)
        {
            _db = db;
            _env = env;
            _emailSender = emailSender;
        }

        // GET: /organizer/events
        [HttpGet("events")]
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var events = await _db.Events
                .Include(e => e.Category)
                .Where(e => e.OrganizerId == userId)
                .OrderByDescending(e => e.StartAt)
                .ToListAsync();

            return View(events);
        }

        // GET: /organizer/events/create
        [HttpGet("events/create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(new Event { StartAt = DateTime.UtcNow.AddDays(1) });
        }

        // POST: /organizer/events/create
        [HttpPost("events/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model, IFormFile poster)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Basic custom validation
            if (model.StartAt <= DateTime.UtcNow)
                ModelState.AddModelError(nameof(model.StartAt), "Дата має бути в майбутньому.");
            var localHour = model.StartAt.ToLocalTime().Hour;
            if (localHour < 7 || localHour >= 23)
                ModelState.AddModelError(nameof(model.StartAt), "Подія має відбуватися між 07:00 та 23:00.");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                return View(model);
            }

            // Poster handling
            if (poster != null)
            {
                var allowed = new[] { "image/jpeg", "image/png" };
                if (!allowed.Contains(poster.ContentType))
                {
                    ModelState.AddModelError("poster", "Потрібен JPEG або PNG.");
                    ViewBag.Categories = await _db.Categories.ToListAsync();
                    return View(model);
                }

                var uploads = Path.Combine(_env.WebRootPath, "uploads", "posters");
                Directory.CreateDirectory(uploads);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(poster.FileName)}";
                var filePath = Path.Combine(uploads, fileName);
                await using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await poster.CopyToAsync(fs);
                }
                model.PosterPath = $"/uploads/posters/{fileName}";
            }

            model.OrganizerId = userId;
            _db.Events.Add(model);
            await _db.SaveChangesAsync();

            // Optional: send confirmation to organizer
            var organizer = await _db.Users.FindAsync(userId);
            if (organizer != null)
            {
                await _emailSender.SendEmailAsync(organizer.Email, "Event created",
                    $"Your event '{model.Title}' has been created.");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /organizer/events/edit/5
        [HttpGet("events/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var ev = await _db.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();

            if (ev.OrganizerId != userId) return Forbid();

            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(ev);
        }

        // POST: /organizer/events/edit/5
        [HttpPost("events/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event model, IFormFile poster)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var ev = await _db.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();
            if (ev.OrganizerId != userId) return Forbid();

            // Capacity validation: cannot set capacity less than already registered
            var registeredCount = ev.Registrations?.Count ?? 0;
            if (model.Capacity < registeredCount)
            {
                ModelState.AddModelError(nameof(model.Capacity), $"Ліміт не може бути меншим за вже зареєстрованих ({registeredCount}).");
            }

            if (model.StartAt <= DateTime.UtcNow)
                ModelState.AddModelError(nameof(model.StartAt), "Дата має бути в майбутньому.");
            var localHour = model.StartAt.ToLocalTime().Hour;
            if (localHour < 7 || localHour >= 23)
                ModelState.AddModelError(nameof(model.StartAt), "Подія має відбуватися між 07:00 та 23:00.");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                return View(model);
            }

            // Update fields (only allowed ones)
            ev.Title = model.Title;
            ev.StartAt = model.StartAt;
            ev.Capacity = model.Capacity;
            ev.CategoryId = model.CategoryId;
            ev.IsDeleted = model.IsDeleted; // organizer shouldn't normally toggle this

            // Poster update
            if (poster != null)
            {
                var allowed = new[] { "image/jpeg", "image/png" };
                if (!allowed.Contains(poster.ContentType))
                {
                    ModelState.AddModelError("poster", "Потрібен JPEG або PNG.");
                    ViewBag.Categories = await _db.Categories.ToListAsync();
                    return View(model);
                }

                var uploads = Path.Combine(_env.WebRootPath, "uploads", "posters");
                Directory.CreateDirectory(uploads);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(poster.FileName)}";
                var filePath = Path.Combine(uploads, fileName);
                await using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await poster.CopyToAsync(fs);
                }
                ev.PosterPath = $"/uploads/posters/{fileName}";
            }

            _db.Events.Update(ev);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /organizer/events/{id}/registrations
        [HttpGet("events/{id:int}/registrations")]
        public async Task<IActionResult> Registrations(int id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var ev = await _db.Events
                .Include(e => e.Registrations).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();
            if (ev.OrganizerId != userId) return Forbid();

            return View(ev);
        }

        // Optional: quick view of a single event (organizer)
        // GET: /organizer/events/view/5
        [HttpGet("events/view/{id:int}")]
        public async Task<IActionResult> ViewEvent(int id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var ev = await _db.Events
                .Include(e => e.Category)
                .Include(e => e.Registrations).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();
            if (ev.OrganizerId != userId) return Forbid();

            return View("Details", ev); // reuse Events/Details view if present
        }
    }
}
