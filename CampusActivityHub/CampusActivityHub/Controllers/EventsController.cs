using CampusActivityHub.Data;
using CampusActivityHub.Models;
using CampusActivityHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("events")]
public class EventsController : Controller
{
    private readonly AppDbContext _db;
    private readonly IEmailSender _emailSender;
    private readonly IWebHostEnvironment _env;

    public EventsController(AppDbContext db, IEmailSender emailSender, IWebHostEnvironment env)
    {
        _db = db;
        _emailSender = emailSender;
        _env = env;
    }

    // GET: /events
    [HttpGet("")]
    public async Task<IActionResult> Index(string q = null)
    {
        var query = _db.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(e => e.Title.Contains(q));

        var list = await query.OrderBy(e => e.StartAt).ToListAsync();
        return View(list);
    }

    // GET: /events/create
    [Authorize(Roles = "Organizer")]
    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _db.Categories.ToListAsync();
        return View(new Event());
    }

    // POST: /events/create
    [Authorize(Roles = "Organizer")]
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event model, IFormFile poster, string[] tags)
    {
        // Custom validators
        if (model.StartAt <= DateTime.UtcNow)
            ModelState.AddModelError(nameof(model.StartAt), "Дата має бути в майбутньому.");
        var hour = model.StartAt.ToLocalTime().Hour;
        if (hour < 7 || hour >= 23)
            ModelState.AddModelError(nameof(model.StartAt), "Подія має відбуватися між 07:00 та 23:00.");

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(model);
        }

        // Poster validation & save
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

        // Organizer (simple cookie-based user id)
        var userId = User.Identity?.Name ?? "system";
        model.OrganizerId = userId;
        _db.Events.Add(model);
        await _db.SaveChangesAsync();

        // Send email to organizer (async)
        await _emailSender.SendEmailAsync(model.Organizer?.Email ?? "organizer@example.com",
            "Event created", $"Your event '{model.Title}' was created.");

        return RedirectToAction(nameof(Index));
    }

    // GET: /events/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var ev = await _db.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .Include(e => e.Registrations).ThenInclude(r => r.User)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null) return NotFound();
        return View(ev);
    }

    // POST: /events/{id}/register (AJAX)
    [Authorize(Roles = "Student")]
    [HttpPost("{id:int}/register")]
    public async Task<IActionResult> RegisterAsync(int id)
    {
        var userId = User.Identity?.Name;
        if (userId == null) return Unauthorized();

        var ev = await _db.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
        if (ev == null) return NotFound();
        if (ev.StartAt <= DateTime.UtcNow) return BadRequest("Подія вже відбулася.");
        if (ev.Registrations.Count >= ev.Capacity) return BadRequest("Подія заповнена.");
        if (ev.Registrations.Any(r => r.UserId == userId)) return BadRequest("Ви вже зареєстровані.");

        var reg = new Registration
        {
            EventId = id,
            UserId = userId,
            RegistrationDate = DateTime.UtcNow
        };
        _db.Registrations.Add(reg);
        await _db.SaveChangesAsync();

        return Ok(new { success = true });
    }

    // Admin soft-delete
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();
        ev.IsDeleted = true;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Search async example
    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync(string q)
    {
        var results = await _db.Events
            .Where(e => e.Title.Contains(q))
            .OrderBy(e => e.StartAt)
            .Take(20)
            .ToListAsync();
        return PartialView("_EventListPartial", results);
    }

    // ByCategory route
    [HttpGet("category/{categoryName}")]
    public async Task<IActionResult> ByCategory(string categoryName)
    {
        var list = await _db.Events
            .Include(e => e.Category)
            .Where(e => e.Category.Name == categoryName)
            .ToListAsync();
        return View("Index", list);
    }

    // ByDate route
    [HttpGet("{year:int}/{month:int}")]
    public async Task<IActionResult> ByDate(int year, int month)
    {
        var list = await _db.Events
            .Where(e => e.StartAt.Year == year && e.StartAt.Month == month)
            .ToListAsync();
        return View("Index", list);
    }
}
