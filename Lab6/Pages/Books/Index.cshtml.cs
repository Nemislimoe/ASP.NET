using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;

namespace Lab6.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<Book> Books { get; set; } = new List<Book>();

        public async Task OnGetAsync()
        {
            Books = await _context.Books.AsNoTracking().ToListAsync();
        }
    }
}
