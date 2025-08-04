using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Application.Interfaces.Infrastructure;
using DigitekShop.Infrastructure.Email;
using DigitekShop.Infrastructure.LocalStorage;
using System.IO;

namespace DigitekShop.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Email Settings from configuration
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            
            // Register LocalStorage Settings from configuration
            services.Configure<LocalStorageOptions>(configuration.GetSection("LocalStorage"));
            
            // Register Email Template Service
            services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
            
            // Register LocalStorage Service
            services.AddSingleton<ILocalStorageService, LocalStorageService>();
            
            // Register Email Service - Choose one implementation based on your needs
            
            // Production Environment: Choose one of these implementations
            // For SendGrid
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            
            // For SMTP (uncomment if you prefer SMTP over SendGrid)
            // services.AddTransient<IEmailSender, SmtpEmailSender>();
            
            // Test Environment: Use MockEmailSender for testing
            // Uncomment the line below and comment out the production implementation when running tests
            // services.AddTransient<IEmailSender, MockEmailSender>();

            return services;
        }
    }
}