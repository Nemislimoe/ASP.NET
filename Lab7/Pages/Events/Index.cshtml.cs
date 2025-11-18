using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;

namespace Lab7.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<Event> Events { get; set; } = new List<Event>();

        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public int TotalPages { get; set; }
        public const int PageSize = 10;

        public async Task OnGetAsync()
        {
            var q = _context.Events
                .Include(e => e.EventParticipants)
                    .ThenInclude(ep => ep.Participant)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Query))
                q = q.Where(e => e.Title.Contains(Query) || e.Location.Contains(Query));

            // sort by participant count desc
            q = q.OrderByDescending(e => e.EventParticipants.Count);

            var total = await q.CountAsync();
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);
            if (PageIndex < 1) PageIndex = 1;
            if (PageIndex > TotalPages) PageIndex = TotalPages == 0 ? 1 : TotalPages;

            Events = await q.Skip((PageIndex - 1) * PageSize).Take(PageSize).AsNoTracking().ToListAsync();
        }
    }
}
