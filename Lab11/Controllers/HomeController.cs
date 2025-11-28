using Microsoft.AspNetCore.Mvc;

namespace Lab11.Controllers
{
    public class HomeController : Controller
    {
        // Приймає необов'язковий id і повертає View
        public IActionResult Index(int? id)
        {
            ViewData["Id"] = id;
            return View();
        }
    }
}
