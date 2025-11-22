using Microsoft.AspNetCore.Mvc;
using Lab9.Models;

namespace Lab9.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            if (!ModelState.IsValid) return View(model);
            return View("RegisterSuccess", model);
        }
    }
}
