using DigitekShop.Application.Interfaces.Infrastructure;
using DigitekShop.Infrastructure.Configuration;
using DigitekShop.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitekShop.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Email Settings
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));
            
            // Register Email Service
            services.AddTransient<IEmailSender, EmailSender>();
            
            return services;
        }
    }
}