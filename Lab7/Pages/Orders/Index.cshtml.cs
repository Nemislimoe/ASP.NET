using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;

namespace Lab7.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<Order> Orders { get; set; } = new List<Order>();

        [BindProperty(SupportsGet = true)]
        public string? CustomerFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "date_desc";

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public int TotalPages { get; set; }

        public const int PageSize = 5;

        public async Task OnGetAsync()
        {
            var q = _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(CustomerFilter))
                q = q.Where(o => o.CustomerName.Contains(CustomerFilter));

            q = SortOrder switch
            {
                "date_asc" => q.OrderBy(o => o.OrderDate),
                _ => q.OrderByDescending(o => o.OrderDate)
            };

            var total = await q.CountAsync();
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);
            if (PageIndex < 1) PageIndex = 1;
            if (PageIndex > TotalPages) PageIndex = TotalPages == 0 ? 1 : TotalPages;

            Orders = await q
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
