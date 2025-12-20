using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Services;
using OnlineShop.Models;

namespace OnlineShop.Pages.Cart;

public class IndexModel : PageModel
{
    private readonly CartService _cartService;

    public IndexModel(CartService cartService) => _cartService = cartService;

    public List<CartItem> Items { get; set; } = new();

    public void OnGet() => Items = _cartService.GetCart();

    public IActionResult OnPostUpdate(int productId, int quantity)
    {
        _cartService.UpdateQuantity(productId, quantity);
        return RedirectToPage();
    }

    public IActionResult OnPostRemove(int productId)
    {
        _cartService.Remove(productId);
        return RedirectToPage();
    }
}
