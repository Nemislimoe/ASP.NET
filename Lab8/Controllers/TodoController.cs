using Lab8.Data;
using Lab8.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab8.Controllers
{
    public class TodoController : Controller
    {
        private readonly InMemoryStore _store;
        public TodoController(InMemoryStore store) => _store = store;

        public IActionResult Index() => View(_store.GetTodos());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                TempData["Error"] = "Назва завдання не може бути порожньою";
                return RedirectToAction(nameof(Index));
            }
            _store.AddTodo(title.Trim());
            TempData["Success"] = "Завдання додано";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleComplete(int id)
        {
            var t = _store.ToggleTodo(id);
            if (t == null) TempData["Error"] = "Завдання не знайдено";
            else TempData["Success"] = "Статус змінено";
            return RedirectToAction(nameof(Index));
        }
    }
}
