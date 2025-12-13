using Microsoft.AspNetCore.Mvc;

namespace CampusActivityHubPRO.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Перенаправляем на Events/Index
            return RedirectToAction("Index", "Events");
        }
    }
}
