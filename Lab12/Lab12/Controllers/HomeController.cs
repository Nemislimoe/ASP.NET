using Microsoft.AspNetCore.Mvc;

namespace Lab12.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Ласкаво просимо в Lab12";
            ViewBag.Number = 42;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
