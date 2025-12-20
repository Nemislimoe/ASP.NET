using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Threading.Tasks;

namespace OnlineShop.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public Product? Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _db.Products.FindAsync(id);
            if (Product == null) return RedirectToPage("./Products");
            return Page();
        }
    }
}
