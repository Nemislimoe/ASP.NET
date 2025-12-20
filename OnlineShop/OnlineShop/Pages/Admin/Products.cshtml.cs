using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Pages.Admin;

public class ProductsModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public ProductsModel(ApplicationDbContext db) => _db = db;

    public IList<Product> Products { get; set; } = new List<Product>();

    public async Task OnGetAsync()
    {
        Products = await _db.Products.ToListAsync();
    }
}
