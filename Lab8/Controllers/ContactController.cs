using Lab8.Data;
using Lab8.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab8.Controllers
{
    public class ContactController : Controller
    {
        private readonly InMemoryStore _store;
        public ContactController(InMemoryStore store) => _store = store;

        [HttpGet]
        public IActionResult Index() => View(new ContactForm());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(ContactForm model)
        {
            if (!ModelState.IsValid) return View("Index", model);
            _store.SaveContact(model);
            TempData["Success"] = "Ваше повідомлення відправлено";
            return RedirectToAction(nameof(Result));
        }

        public IActionResult Result()
        {
            var last = _store.GetLastContact();
            if (last == null) return RedirectToAction(nameof(Index));
            return View(last);
        }
    }
}
