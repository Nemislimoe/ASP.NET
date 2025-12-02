using CampusActivityHub.Data;
using CampusActivityHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CampusActivityHub.Filters;
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly AppDbContext _db;
    public GlobalExceptionFilter(AppDbContext db) => _db = db;

    public void OnException(ExceptionContext context)
    {
        var ex = context.Exception;

        var log = new ErrorLog
        {
            OccurredAt = DateTime.UtcNow,
            Url = context.HttpContext.Request.Path,
            Message = ex?.Message ?? "No message",
            StackTrace = ex?.StackTrace ?? ex?.ToString() ?? "No stack trace available",
            UserId = context.HttpContext.User?.Identity?.Name
        };

        try
        {
            _db.ErrorLogs.Add(log);
            _db.SaveChanges();
        }
        catch (Exception saveEx)
        {
            // Не кидаємо виняток далі — записуємо у Debug/Console як fallback
            System.Diagnostics.Debug.WriteLine("Failed to save ErrorLog: " + saveEx);
        }

        context.Result = new RedirectToActionResult("Error", "Home", null);
        context.ExceptionHandled = true;
    }
}
