using OnlineShop.Models;

namespace OnlineShop.Services;

public class CartService
{
    private const string SessionKey = "Cart";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext!.Session;

    public List<CartItem> GetCart() => Session.GetObject<List<CartItem>>(SessionKey) ?? new List<CartItem>();

    public void AddToCart(Product product, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == product.Id);
        if (item == null)
        {
            cart.Add(new CartItem { ProductId = product.Id, Name = product.Name, Price = product.Price, Quantity = quantity });
        }
        else
        {
            item.Quantity += quantity;
        }
        Session.SetObject(SessionKey, cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0) cart.Remove(item);
            else item.Quantity = quantity;
            Session.SetObject(SessionKey, cart);
        }
    }

    public void Remove(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(i => i.ProductId == productId);
        Session.SetObject(SessionKey, cart);
    }

    public void Clear()
    {
        Session.Remove(SessionKey);
    }
}
