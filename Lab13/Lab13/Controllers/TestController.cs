using Microsoft.AspNetCore.Mvc;
using Lab13.Services;

namespace Lab13.Controllers
{
    public class TestController : Controller
    {
        private readonly IRandomNumberService _randomService;

        public TestController(IRandomNumberService randomService)
        {
            _randomService = randomService;
        }

        public IActionResult Index()
        {
            ViewBag.Random = _randomService.Next();
            return View();
        }
    }
}
