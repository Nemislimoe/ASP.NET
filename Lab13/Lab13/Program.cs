using Lab13.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Реєстрація сервісів
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IDateTimeService, DateTimeService>();
builder.Services.AddSingleton<IRandomNumberService, RandomNumberService>();
builder.Services.AddScoped<ICalculatorService, CalculatorService>();

// Для завдання 5: зареєструйте FakeEmailService замість EmailService
// Щоб переключити на реальний EmailService, змініть на AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailService, FakeEmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
