using Microsoft.AspNetCore.Mvc;
using Lab10.Models;
using System;

namespace Lab10.Controllers
{
    public class FeedbackController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(Feedback model)
        {
            if (!string.IsNullOrEmpty(model.Message) &&
                model.Message.IndexOf("bad", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ModelState.AddModelError(nameof(model.Message), "Повідомлення не може містити заборонені слова");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Збереження фідбеку
            return RedirectToAction("Thanks");
        }

        public IActionResult Thanks() => View();
    }
}
