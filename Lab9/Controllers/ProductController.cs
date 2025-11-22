using Microsoft.AspNetCore.Mvc;
using Lab9.Models;

namespace Lab9.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model)
        {
            if (!ModelState.IsValid) return View(model);
            return View("AddSuccess", model);
        }
    }
}
