using Lab16.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab16.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;
    }

    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Products.Any()) return;

            db.Products.AddRange(
                new Product { Name = "Apple iPhone 14", Description = "Smartphone", Price = 799.99m, Quantity = 10 },
                new Product { Name = "Samsung Galaxy S23", Description = "Android phone", Price = 699.50m, Quantity = 8 },
                new Product { Name = "Logitech Mouse", Description = "Wireless mouse", Price = 29.99m, Quantity = 50 },
                new Product { Name = "Mechanical Keyboard", Description = "RGB keyboard", Price = 119.00m, Quantity = 20 }
            );

            db.SaveChanges();
        }
    }
}
