using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Lab4.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

var app = builder.Build();

app.UseStaticFiles();

app.Use(async (context, next) =>
{
    context.Items["Notifications"] = new List<NotificationMessage>
    {
        new NotificationMessage { Type = NotificationType.Success, Text = "Welcome to Lab4" }
    };
    await next();
});

app.MapRazorPages();

app.Run();
