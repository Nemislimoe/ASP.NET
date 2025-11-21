using Microsoft.AspNetCore.Mvc;

namespace Lab8.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
