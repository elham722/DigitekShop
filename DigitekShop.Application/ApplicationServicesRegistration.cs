using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using DigitekShop.Application.Features;
using DigitekShop.Application.Services;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Validators.Identity;
using DigitekShop.Domain.Services;

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
            services.AddScoped<OrderDomainService>();

            // Register Application Services
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            // Fix for CS1503: Use a lambda to configure MediatRServiceConfiguration
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register FluentValidation validators
            services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

            return services;
        }
    }
}
