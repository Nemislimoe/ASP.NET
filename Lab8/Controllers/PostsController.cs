using Lab8.Data;
using Lab8.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab8.Controllers
{
    public class PostsController : Controller
    {
        private readonly InMemoryStore _store;
        public PostsController(InMemoryStore store) => _store = store;

        public IActionResult Index() => View(_store.GetPosts());

        public IActionResult Details(int id)
        {
            var p = _store.GetPost(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpGet]
        public IActionResult Create() => View(new Post());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post model)
        {
            if (!ModelState.IsValid) return View(model);
            _store.AddPost(model);
            TempData["Success"] = "Пост успішно створено";
            return RedirectToAction(nameof(Index));
        }
    }
}
