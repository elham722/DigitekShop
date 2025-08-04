using Microsoft.AspNetCore.Http;
using DigitekShop.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DigitekShop.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static IQueryDispatcher QueryDispatcher(this HttpContext context)
        {
            return context.RequestServices.GetRequiredService<IQueryDispatcher>();
        }

        public static ICommandDispatcher CommandDispatcher(this HttpContext context)
        {
            return context.RequestServices.GetRequiredService<ICommandDispatcher>();
        }
    }
} 