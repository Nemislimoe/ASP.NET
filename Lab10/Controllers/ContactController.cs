using Microsoft.AspNetCore.Mvc;
using Lab10.Models;

namespace Lab10.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Send() => View();

        [HttpPost]
        public IActionResult Send(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Логіка відправки повідомлення
            return RedirectToAction("Index", "Home");
        }
    }
}
