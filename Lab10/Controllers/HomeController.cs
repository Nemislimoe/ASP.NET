using Microsoft.AspNetCore.Mvc;

namespace Lab10.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
