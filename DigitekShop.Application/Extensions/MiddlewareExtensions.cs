using Microsoft.AspNetCore.Builder;
using DigitekShop.Application.Middleware;

namespace DigitekShop.Application.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
} 