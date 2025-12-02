using Microsoft.AspNetCore.Mvc;

namespace CampusActivityHub.Controllers;
public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult About() => View();
    public IActionResult Privacy() => View();
    public IActionResult Error() => View();
}
