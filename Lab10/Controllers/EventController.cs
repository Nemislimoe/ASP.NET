using Microsoft.AspNetCore.Mvc;
using Lab10.Models;

namespace Lab10.Controllers
{
    public class EventController : Controller
    {
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(EventRegistration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Логіка реєстрації
            return RedirectToAction("Index", "Home");
        }
    }
}
