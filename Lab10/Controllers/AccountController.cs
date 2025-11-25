using Microsoft.AspNetCore.Mvc;
using Lab10.Models;

namespace Lab10.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(UserRegistration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Тут можна додати логіку збереження користувача
            return RedirectToAction("Index", "Home");
        }
    }
}
