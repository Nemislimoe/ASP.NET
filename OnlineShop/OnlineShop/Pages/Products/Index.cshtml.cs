using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public IList<Product> Products { get; set; } = new List<Product>();

        // Підтримка прив'язки з query string
        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Sort { get; set; }

        // Список категорій для селекту
        public SelectList CategoryList { get; set; } = null!;

        public async Task OnGetAsync()
        {
            // Отримуємо всі категорії окремо (щоб селект завжди мав повний набір)
            var categories = await _db.Products
                .Where(p => !string.IsNullOrEmpty(p.Category))
                .Select(p => p.Category!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            CategoryList = new SelectList(categories);

            // Базовий запит
            var query = _db.Products.AsQueryable();

            // Фільтр за категорією (якщо задано)
            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(p => p.Category == Category);
            }

            // Сортування
            Products = Sort switch
            {
                "price_asc" => await query.OrderBy(p => p.Price).ToListAsync(),
                "price_desc" => await query.OrderByDescending(p => p.Price).ToListAsync(),
                _ => await query.ToListAsync()
            };
        }
    }
}