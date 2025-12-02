using CampusActivityHub.Data;
using CampusActivityHub.Filters;
using CampusActivityHub.Middleware;
using CampusActivityHub.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Configuration: DB (SQLite by default)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=campus.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();


// MVC + global filters
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<RequestLoggingFilter>();
    options.Filters.Add<GlobalExceptionFilter>();
});

// Simple cookie-based auth for demo purposes
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("OrganizerOnly", policy => policy.RequireRole("Organizer"));
});

// Email sender service
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

var app = builder.Build();

// Ensure DB created and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await SeedData.Initialize(services);
}

// Middleware pipeline
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestTimingMiddleware>();

// Custom routes
app.MapControllerRoute(
    name: "events_by_category",
    pattern: "events/category/{categoryName}",
    defaults: new { controller = "Events", action = "ByCategory" });

app.MapControllerRoute(
    name: "events_by_date",
    pattern: "events/{year:int}/{month:int}",
    defaults: new { controller = "Events", action = "ByDate" });

app.MapControllerRoute(
    name: "event_details",
    pattern: "event/{id:int}/{slug?}",
    defaults: new { controller = "Events", action = "Details" });

app.MapControllerRoute(
    name: "organizer_events",
    pattern: "organizer/{organizerId}/events",
    defaults: new { controller = "Organizer", action = "Index" });

app.MapDefaultControllerRoute();

app.Run();
