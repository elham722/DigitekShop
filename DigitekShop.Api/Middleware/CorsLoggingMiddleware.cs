using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DigitekShop.Api.Middleware
{
    public class CorsLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorsLoggingMiddleware> _logger;

        public CorsLoggingMiddleware(RequestDelegate next, ILogger<CorsLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var origin = context.Request.Headers["Origin"].FirstOrDefault();
            var method = context.Request.Method;
            var path = context.Request.Path;

            if (!string.IsNullOrEmpty(origin))
            {
                _logger.LogInformation("CORS Request: {Method} {Path} from {Origin}", method, path, origin);
            }

            await _next(context);

            // Log CORS response headers
            var corsHeaders = context.Response.Headers
                .Where(h => h.Key.StartsWith("Access-Control-"))
                .ToDictionary(h => h.Key, h => h.Value.ToString());

            if (corsHeaders.Any())
            {
                _logger.LogDebug("CORS Response Headers: {@CorsHeaders}", corsHeaders);
            }
        }
    }

    public static class CorsLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorsLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsLoggingMiddleware>();
        }
    }
} 