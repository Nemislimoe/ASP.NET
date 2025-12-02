using CampusActivityHub.Data;
using CampusActivityHub.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CampusActivityHub.Filters;
public class RequestLoggingFilter : IAsyncActionFilter
{
    private readonly AppDbContext _db;
    public RequestLoggingFilter(AppDbContext db) => _db = db;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var start = DateTime.UtcNow;
        var executed = await next();
        var duration = DateTime.UtcNow - start;

        var log = new ErrorLog
        {
            OccurredAt = DateTime.UtcNow,
            Url = context.HttpContext.Request.Path,
            Message = $"Action executed in {duration.TotalMilliseconds} ms",
            StackTrace = null,
            UserId = context.HttpContext.User.Identity?.Name
        };
        _db.ErrorLogs.Add(log);
        await _db.SaveChangesAsync();
    }
}
