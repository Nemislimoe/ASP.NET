using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public string? Category { get; set; }
}
