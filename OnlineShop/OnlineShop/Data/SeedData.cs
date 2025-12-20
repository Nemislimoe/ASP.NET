using OnlineShop.Models;

namespace OnlineShop.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext db)
    {
        if (db.Products.Any()) return;

        var products = new List<Product>
        {
            new() { Name = "Кавоварка", Description = "Електрична кавоварка", Price = 1299.99m, StockQuantity = 10, Category = "Побутова техніка" },
            new() { Name = "Навушники", Description = "Бездротові навушники", Price = 499.50m, StockQuantity = 25, Category = "Електроніка" },
            new() { Name = "Чашка", Description = "Керамічна чашка 350мл", Price = 79.00m, StockQuantity = 100, Category = "Посуд" },
            new() { Name = "Ноутбук", Description = "14'' ноутбук, 8GB RAM", Price = 15999.00m, StockQuantity = 5, Category = "Електроніка" },
            new() { Name = "Рюкзак", Description = "Водонепроникний рюкзак", Price = 899.00m, StockQuantity = 30, Category = "Аксесуари" }
        };

        db.Products.AddRange(products);
        db.SaveChanges();
    }
}
