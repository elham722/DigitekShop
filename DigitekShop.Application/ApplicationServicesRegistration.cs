using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using DigitekShop.Application.Profiles;
using DigitekShop.Application.Features;

namespace DigitekShop.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureAddApplicationServices(this IServiceCollection services)
        {
            // Specify assemblies to scan for AutoMapper profiles
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            // Register Features (Commands and Queries)
            services.AddFeatures();

            return services;
        }
    }
}
