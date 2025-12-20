using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Checkout;

public class IndexModel : PageModel
{
    private readonly CartService _cartService;
    private readonly OrderService _orderService;

    public IndexModel(CartService cartService, OrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    [BindProperty]
    public CheckoutInput Input { get; set; } = new();

    public List<CartItem> Items { get; set; } = new();

    public void OnGet()
    {
        Items = _cartService.GetCart();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Items = _cartService.GetCart();
        if (!ModelState.IsValid) return Page();
        if (!Items.Any())
        {
            ModelState.AddModelError(string.Empty, "Корзина порожня.");
            return Page();
        }

        var customer = new Customer
        {
            FirstName = Input.FirstName,
            LastName = Input.LastName,
            Email = Input.Email
        };

        var order = await _orderService.CreateOrderAsync(customer, Items);
        _cartService.Clear();

        return RedirectToPage("/Index");
    }

    public class CheckoutInput
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string FirstName { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required]
        public string LastName { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.EmailAddress]
        public string Email { get; set; } = null!;
    }
}
