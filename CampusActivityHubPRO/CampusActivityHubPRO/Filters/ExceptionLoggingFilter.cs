using CampusActivityHubPRO.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CampusActivityHubPRO.Filters
{
    public class ExceptionLoggingFilter : IExceptionFilter
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ExceptionLoggingFilter> _logger;
        private readonly IHttpContextAccessor _http;

        public ExceptionLoggingFilter(ApplicationDbContext db, ILogger<ExceptionLoggingFilter> logger, IHttpContextAccessor http)
        {
            _db = db;
            _logger = logger;
            _http = http;
        }

        public void OnException(ExceptionContext context)
        {
            var path = _http.HttpContext?.Request.Path;
            var method = _http.HttpContext?.Request.Method;
            var user = _http.HttpContext?.User?.Identity?.Name;

            var log = new ErrorLog
            {
                Path = path,
                Method = method,
                UserId = user,
                Exception = context.Exception.ToString()
            };

            try
            {
                _db.ErrorLogs.Add(log);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save error log to DB");
            }

            _logger.LogError(context.Exception, "Unhandled exception on {Path}", path);
        }
    }
}
