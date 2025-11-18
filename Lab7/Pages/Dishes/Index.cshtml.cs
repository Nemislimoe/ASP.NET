using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab7.Pages.Dishes
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<Dish> Dishes { get; set; } = new List<Dish>();
        public IList<Category> Categories { get; set; } = new List<Category>();

        [BindProperty(SupportsGet = true)] public int? CategoryId { get; set; }
        [BindProperty(SupportsGet = true)] public string SortOrder { get; set; } = "price_asc";

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.AsNoTracking().ToListAsync();

            var q = _context.Dishes
                .Include(d => d.Restaurant)
                .Include(d => d.DishCategories).ThenInclude(dc => dc.Category)
                .AsQueryable();

            if (CategoryId.HasValue)
                q = q.Where(d => d.DishCategories.Any(dc => dc.CategoryId == CategoryId.Value));

            q = SortOrder switch
            {
                "price_desc" => q.OrderByDescending(d => d.Price),
                _ => q.OrderBy(d => d.Price)
            };

            Dishes = await q.AsNoTracking().ToListAsync();
        }
    }
}
