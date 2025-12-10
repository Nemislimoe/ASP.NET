using Microsoft.AspNetCore.Mvc;
using Lab13.Services;

namespace Lab13.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _email;

        public EmailController(IEmailService email)
        {
            _email = email;
        }

        [HttpGet]
        public IActionResult Send() => View();

        [HttpPost]
        public IActionResult Send(string to, string subject, string body)
        {
            var result = _email.Send(to, subject, body);
            ViewBag.Result = result;
            return View();
        }
    }
}
