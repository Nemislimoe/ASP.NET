using CampusActivityHubPRO.Filters;
using CampusActivityHubPRO.Data;
using CampusActivityHubPRO.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register IHttpContextAccessor for filters that need HttpContext
builder.Services.AddHttpContextAccessor();

// Register filters in DI
builder.Services.AddScoped<RequestLoggingFilter>();
builder.Services.AddScoped<ExceptionLoggingFilter>();

// MVC + runtime compilation
builder.Services.AddControllersWithViews(options =>
{
    // Add filters via DI
    options.Filters.AddService<RequestLoggingFilter>();
    options.Filters.AddService<ExceptionLoggingFilter>();
}).AddRazorRuntimeCompilation();

// Services
builder.Services.AddScoped<IEmailSender, FakeEmailSender>();

var app = builder.Build();

// Ensure DB and seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    await ApplicationDbContext.SeedAsync(services);
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "events_by_category",
    pattern: "events/category/{categoryName}",
    defaults: new { controller = "Events", action = "ByCategory" });

app.MapControllerRoute(
    name: "events_by_date",
    pattern: "events/{year:int}/{month:int}",
    defaults: new { controller = "Events", action = "ByMonth" });

app.MapControllerRoute(
    name: "event_detail",
    pattern: "event/{id:int}/{slug?}",
    defaults: new { controller = "Events", action = "Details" });

app.MapControllerRoute(
    name: "organizer_events",
    pattern: "organizer/{organizerId}/events",
    defaults: new { controller = "Events", action = "ByOrganizer" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
