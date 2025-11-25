using Microsoft.AspNetCore.Mvc;
using Lab10.Models;

namespace Lab10.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Збереження товару (імітація)
            return RedirectToAction("Index", "Home");
        }
    }
}
