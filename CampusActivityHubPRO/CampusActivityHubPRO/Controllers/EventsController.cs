using CampusActivityHubPRO.Data;
using CampusActivityHubPRO.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusActivityHubPRO.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _email;

        public EventsController(ApplicationDbContext db, IEmailSender email)
        {
            _db = db;
            _email = email;
        }

        // GET: /Events
        public async Task<IActionResult> Index()
        {
            var events = await _db.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .OrderBy(e => e.StartAt)
                .ToListAsync();
            return View(events);
        }

        // GET: /events/category/{categoryName}
        public async Task<IActionResult> ByCategory(string categoryName)
        {
            var events = await _db.Events
                .Include(e => e.Category)
                .Where(e => e.Category!.Name == categoryName)
                .ToListAsync();
            return View("Index", events);
        }

        // GET: /events/{year}/{month}
        public async Task<IActionResult> ByMonth(int year, int month)
        {
            var events = await _db.Events
                .Where(e => e.StartAt.Year == year && e.StartAt.Month == month)
                .ToListAsync();
            return View("Index", events);
        }

        // GET: /event/{id}/{slug}
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _db.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .Include(e => e.EventTags!)
                    .ThenInclude(et => et.Tag)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();
            return View(ev);
        }

        // AJAX: tag autocomplete
        [HttpGet]
        [Route("api/tags/autocomplete")]
        public async Task<IActionResult> TagAutocomplete(string q)
        {
            var tags = await _db.Tags
                .Where(t => t.Name.Contains(q))
                .Select(t => t.Name)
                .Take(10)
                .ToListAsync();
            return Json(tags);
        }

        // Student registration (async)
        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("api/events/{id}/register")]
        public async Task<IActionResult> RegisterAjax(int id, [FromForm] string? comment)
        {
            var ev = await _db.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();

            if (ev.StartAt <= DateTime.UtcNow) return BadRequest("Event already started.");
            if (ev.Registrations != null && ev.Registrations.Count >= ev.Capacity) return BadRequest("Event is full.");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
            var exists = await _db.Registrations.AnyAsync(r => r.EventId == id && r.UserId == userId);
            if (exists) return BadRequest("Already registered.");

            var reg = new Registration { EventId = id, UserId = userId, Comment = comment, RegistrationDate = DateTime.UtcNow };
            _db.Registrations.Add(reg);
            await _db.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}
