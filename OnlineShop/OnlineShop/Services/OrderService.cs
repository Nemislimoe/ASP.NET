using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services;

public class OrderService
{
    private readonly ApplicationDbContext _db;

    public OrderService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Order> CreateOrderAsync(Customer customer, List<CartItem> cartItems)
    {
        // find or create customer
        var existing = await _db.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);
        if (existing == null)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            existing = customer;
        }

        var order = new Order
        {
            CustomerId = existing.Id,
            OrderDate = DateTime.UtcNow,
            TotalAmount = cartItems.Sum(i => i.Price * i.Quantity)
        };

        foreach (var ci in cartItems)
        {
            var product = await _db.Products.FindAsync(ci.ProductId);
            if (product != null)
            {
                var qty = Math.Min(ci.Quantity, product.StockQuantity);
                product.StockQuantity -= qty;
                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = qty,
                    Price = product.Price
                });
            }
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }
}
