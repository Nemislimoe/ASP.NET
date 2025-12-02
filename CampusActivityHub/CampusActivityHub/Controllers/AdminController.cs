using CampusActivityHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;
    public AdminController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Dashboard()
    {
        var eventsByCategory = await _db.Events
            .Include(e => e.Category)
            .GroupBy(e => e.Category.Name)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToListAsync();

        var topEvents = await _db.Events
            .Include(e => e.Registrations)
            .OrderByDescending(e => e.Registrations.Count)
            .Take(5)
            .Select(e => new { e.Title, Count = e.Registrations.Count })
            .ToListAsync();

        var registrationsByMonth = await _db.Registrations
            .GroupBy(r => new { r.RegistrationDate.Year, r.RegistrationDate.Month })
            .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Count = g.Count() })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        ViewBag.EventsByCategory = eventsByCategory;
        ViewBag.TopEvents = topEvents;
        ViewBag.RegistrationsByMonth = registrationsByMonth;
        return View();
    }
}
