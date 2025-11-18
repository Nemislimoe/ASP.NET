using Lab7.Data;
using Lab7.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Seed data and apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // PRODUCTS + ORDERS (many-to-many via OrderProduct)
    if (!db.Products.Any())
    {
        var p1 = new Product { Title = "Apple", Price = 1.00m };
        var p2 = new Product { Title = "Bread", Price = 2.50m };
        var p3 = new Product { Title = "Milk", Price = 1.75m };
        db.Products.AddRange(p1, p2, p3);
        db.SaveChanges();

        var o1 = new Order { CustomerName = "Ivan Petrov", OrderDate = DateTime.UtcNow.AddDays(-2) };
        var o2 = new Order { CustomerName = "Olena Shevchenko", OrderDate = DateTime.UtcNow.AddDays(-1) };

        o1.OrderProducts.Add(new OrderProduct { ProductId = p1.Id, Quantity = 3 });
        o1.OrderProducts.Add(new OrderProduct { ProductId = p2.Id, Quantity = 1 });

        o2.OrderProducts.Add(new OrderProduct { ProductId = p2.Id, Quantity = 2 });
        o2.OrderProducts.Add(new OrderProduct { ProductId = p3.Id, Quantity = 5 });

        db.Orders.AddRange(o1, o2);
        db.SaveChanges();
    }

    // EVENTS + PARTICIPANTS (many-to-many via EventParticipant)
    if (!db.Events.Any())
    {
        var ev1 = new Event { Title = "Tech Meetup", Date = DateTime.UtcNow.AddDays(7), Location = "Kyiv" };
        var ev2 = new Event { Title = "Music Fest", Date = DateTime.UtcNow.AddDays(30), Location = "Lviv" };

        var pa1 = new Participant { FullName = "Petro Ivanov", Email = "petro@example.com" };
        var pa2 = new Participant { FullName = "Maria Kovalenko", Email = "maria@example.com" };
        var pa3 = new Participant { FullName = "Andriy Melnyk", Email = "andriy@example.com" };

        ev1.EventParticipants.Add(new EventParticipant { Participant = pa1 });
        ev1.EventParticipants.Add(new EventParticipant { Participant = pa2 });

        ev2.EventParticipants.Add(new EventParticipant { Participant = pa2 });
        ev2.EventParticipants.Add(new EventParticipant { Participant = pa3 });

        db.Events.AddRange(ev1, ev2);
        db.SaveChanges();
    }

    // RESTAURANTS + DISHES + CATEGORIES (one-to-many and many-to-many via DishCategory)
    if (!db.Restaurants.Any())
    {
        var cat1 = new Category { Name = "Soups" };
        var cat2 = new Category { Name = "Drinks" };
        var cat3 = new Category { Name = "Main" };
        db.Categories.AddRange(cat1, cat2, cat3);
        db.SaveChanges();

        var r1 = new Restaurant { Name = "Sunny Cafe", Address = "Main St 1" };
        var r2 = new Restaurant { Name = "Ocean Grill", Address = "Harbor Ave 5" };

        var d1 = new Dish { Title = "Chicken Soup", Price = 4.50m, Restaurant = r1 };
        d1.DishCategories.Add(new DishCategory { CategoryId = cat1.Id });

        var d2 = new Dish { Title = "Lemonade", Price = 2.00m, Restaurant = r1 };
        d2.DishCategories.Add(new DishCategory { CategoryId = cat2.Id });

        var d3 = new Dish { Title = "Grilled Salmon", Price = 12.00m, Restaurant = r2 };
        d3.DishCategories.Add(new DishCategory { CategoryId = cat3.Id });

        r1.Dishes.Add(d1);
        r1.Dishes.Add(d2);
        r2.Dishes.Add(d3);

        db.Restaurants.AddRange(r1, r2);
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
