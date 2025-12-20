using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly CartService _cartService;

    public DetailsModel(ApplicationDbContext db, CartService cartService)
    {
        _db = db;
        _cartService = cartService;
    }

    public Product? Product { get; set; }

    [BindProperty]
    public int Quantity { get; set; } = 1;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Product = await _db.Products.FindAsync(id);
        if (Product == null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostAddAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        var qty = Math.Max(1, Quantity);
        _cartService.AddToCart(product, qty);
        return RedirectToPage("/Cart/Index");
    }
}
