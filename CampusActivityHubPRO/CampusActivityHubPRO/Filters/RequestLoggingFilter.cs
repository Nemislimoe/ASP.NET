using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CampusActivityHubPRO.Filters
{
    public class RequestLoggingFilter : IActionFilter
    {
        private readonly ILogger<RequestLoggingFilter> _logger;
        private Stopwatch? _sw;

        public RequestLoggingFilter(ILogger<RequestLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _sw = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _sw?.Stop();
            var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
            var path = context.HttpContext.Request.Path;
            var method = context.HttpContext.Request.Method;
            var elapsed = _sw?.ElapsedMilliseconds ?? 0;
            _logger.LogInformation("Request {Method} {Path} by {User} took {Elapsed}ms", method, path, user, elapsed);
        }
    }
}
