using CampusActivityHubPRO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusActivityHubPRO.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Student/MyRegistrations
        [HttpGet]
        public async Task<IActionResult> MyRegistrations()
        {
            var userId = _userManager.GetUserId(User);
            var regs = await _db.Registrations
                .Where(r => r.UserId == userId)
                .Include(r => r.Event!)
                    .ThenInclude(e => e.Category)
                .Include(r => r.Event!)
                    .ThenInclude(e => e.Organizer)
                .ToListAsync();

            return View(regs);
        }

        // POST: /Student/LeaveFeedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveFeedback(int registrationId, string feedback)
        {
            if (string.IsNullOrWhiteSpace(feedback))
            {
                ModelState.AddModelError(nameof(feedback), "Відгук не може бути порожнім.");
            }

            var userId = _userManager.GetUserId(User);
            var reg = await _db.Registrations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.Id == registrationId && r.UserId == userId);

            if (reg == null)
            {
                return NotFound();
            }

            if (reg.Event == null)
            {
                ModelState.AddModelError("", "Пов'язана подія не знайдена.");
            }
            else
            {
                // Перевірка: відгук можна залишити тільки після дати події
                if (reg.Event.EndAt > DateTime.UtcNow)
                {
                    ModelState.AddModelError("", "Ви можете залишити відгук тільки після завершення події.");
                }
            }

            if (!ModelState.IsValid)
            {
                // Повертаємося на сторінку реєстрацій, щоб показати помилки
                TempData["FeedbackErrors"] = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return RedirectToAction(nameof(MyRegistrations));
            }

            reg.Feedback = feedback.Trim();
            await _db.SaveChangesAsync();

            TempData["FeedbackSuccess"] = "Дякуємо за відгук.";
            return RedirectToAction(nameof(MyRegistrations));
        }
    }
}
