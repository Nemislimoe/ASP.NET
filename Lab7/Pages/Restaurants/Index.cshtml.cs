using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab7.Pages.Restaurants
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        [BindProperty(SupportsGet = true)] public string? NameQuery { get; set; }
        [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public const int PageSize = 5;

        public async Task OnGetAsync()
        {
            var q = _context.Restaurants
                .Include(r => r.Dishes)
                    .ThenInclude(d => d.DishCategories)
                        .ThenInclude(dc => dc.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(NameQuery))
                q = q.Where(r => r.Name.Contains(NameQuery));

            var total = await q.CountAsync();
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);
            Restaurants = await q.Skip((PageIndex - 1) * PageSize).Take(PageSize).AsNoTracking().ToListAsync();
        }
    }
}
