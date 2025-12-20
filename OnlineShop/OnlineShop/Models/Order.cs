using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models;

public class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public DateTime OrderDate { get; set; }

    [Range(0, 9999999)]
    public decimal TotalAmount { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}
