using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Threading.Tasks;

namespace OnlineShop.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public Product Product { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _db.Products.Add(Product);
            await _db.SaveChangesAsync();
            return RedirectToPage("./Products");
        }
    }
}
