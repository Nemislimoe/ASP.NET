using Microsoft.AspNetCore.Mvc;

namespace Lab11.Controllers
{
    public class ProductsController : Controller
    {
        // Завдання 2: /Products/Search?name=Book&categoryId=3
        public IActionResult Search(string? name, int? categoryId)
        {
            return Content($"Name={name}, CategoryId={categoryId}");
        }

        // Завдання 3 і 4: маршрут catalog/{category}/{page?}
        // category і page беруться з маршруту, sort з query string
        // /catalog/electronics/2?sort=price
        public IActionResult Category(string category, int? page, string? sort)
        {
            return Content($"Category={category}, Page={page}, Sort={sort}");
        }
    }
}
