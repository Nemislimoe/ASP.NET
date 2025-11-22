using Microsoft.AspNetCore.Mvc;
using Lab9.Models;

namespace Lab9.Controllers
{
    public class TicketController : Controller
    {
        [HttpGet]
        public ActionResult Order()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order(TicketOrder model)
        {
            if (!ModelState.IsValid) return View(model);
            return View("OrderSuccess", model);
        }
    }
}
