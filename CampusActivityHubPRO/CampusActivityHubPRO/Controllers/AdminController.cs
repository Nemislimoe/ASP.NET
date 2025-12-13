using CampusActivityHubPRO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusActivityHubPRO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Events per category
            var eventsByCategory = await _db.Categories
                .Select(c => new { c.Name, Count = c.Events!.Count() })
                .ToListAsync();

            // Top 5 events by registrations
            var topEvents = await _db.Events
                .Select(e => new { e.Title, Count = e.Registrations!.Count })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            // Registrations per month (last 12 months)
            var now = DateTime.UtcNow;
            var months = Enumerable.Range(0, 12)
                .Select(i => new { Month = now.AddMonths(-i).ToString("yyyy-MM"), Date = now.AddMonths(-i) })
                .Reverse()
                .ToList();

            var regs = await _db.Registrations
                .GroupBy(r => new { r.RegistrationDate.Year, r.RegistrationDate.Month })
                .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Count = g.Count() })
                .ToListAsync();

            var regsByMonth = months.Select(m =>
            {
                var parts = m.Month.Split('-');
                var y = int.Parse(parts[0]);
                var mo = int.Parse(parts[1]);
                var found = regs.FirstOrDefault(r => r.Year == y && r.Month == mo);
                return new { Month = m.Month, Count = found?.Count ?? 0 };
            }).ToList();

            ViewBag.EventsByCategory = eventsByCategory;
            ViewBag.TopEvents = topEvents;
            ViewBag.RegsByMonth = regsByMonth;

            return View();
        }

        // Soft delete event
        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            ev.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        // Category management (create)
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Dashboard");
            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }
    }
}
