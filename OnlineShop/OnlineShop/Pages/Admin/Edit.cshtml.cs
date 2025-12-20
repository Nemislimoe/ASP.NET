using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Threading.Tasks;

namespace OnlineShop.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public Product Product { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _db.Products.FindAsync(id);
            if (Product == null) return RedirectToPage("./Products");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var existing = await _db.Products.FindAsync(Product.Id);
            if (existing == null) return NotFound();

            existing.Name = Product.Name;
            existing.Description = Product.Description;
            existing.Price = Product.Price;
            existing.StockQuantity = Product.StockQuantity;
            existing.Category = Product.Category;

            _db.Products.Update(existing);
            await _db.SaveChangesAsync();

            return RedirectToPage("./Products");
        }
    }
}
