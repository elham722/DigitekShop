using DigitekShop.Application.Features;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using DigitekShop.Application.Validators.Identity;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Services;

namespace DigitekShop.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            // Register all validators from the assembly
            services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
            
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            
            return services;
        }

        public static IServiceCollection AddApplicationFeatures(this IServiceCollection services)
        {
            // Register CQRS features
            services.AddFeatures();
            
            return services;
        }
    }
} 