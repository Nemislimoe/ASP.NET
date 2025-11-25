using Microsoft.AspNetCore.Mvc;
using Lab10.Models;

namespace Lab10.Controllers
{
    public class AppointmentController : Controller
    {
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Appointment model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Збереження зустрічі
            return RedirectToAction("Index", "Home");
        }
    }
}
