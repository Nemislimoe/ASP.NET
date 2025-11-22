using Microsoft.AspNetCore.Mvc;
using Lab9.Models;

namespace Lab9.Controllers
{
    public class FeedbackController : Controller
    {
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback model)
        {
            if (!ModelState.IsValid) return View(model);
            return View("CreateSuccess", model);
        }
    }
}
