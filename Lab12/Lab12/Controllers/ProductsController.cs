using Microsoft.AspNetCore.Mvc;
using Lab12.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lab12.Controllers
{
    public class ProductsController : Controller
    {
        // Статичний список в пам'яті
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Книга", Price = 9.99m },
            new Product { Id = 2, Name = "Ручка", Price = 1.50m }
        };

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Info = "Список доступних продуктів";
            return View(_products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            var nextId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            model.Id = nextId;
            _products.Add(model);

            TempData["Success"] = "Продукт створено";
            return RedirectToAction("Index");
        }
    }
}
