using DigitekShop.Api.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace DigitekShop.Api.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsWithConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection("CorsSettings").Get<CorsSettings>();

            services.AddCors(options =>
            {
                // Development Policy
                options.AddPolicy("DevelopmentPolicy", policy =>
                {
                    policy.WithOrigins(corsSettings?.AllowedOrigins ?? new[] { "*" })
                          .WithMethods(corsSettings?.AllowedMethods ?? new[] { "*" })
                          .WithHeaders(corsSettings?.AllowedHeaders ?? new[] { "*" });

                    if (corsSettings?.AllowCredentials == true)
                    {
                        policy.AllowCredentials();
                    }
                    else
                    {
                        policy.DisallowCredentials();
                    }

                    if (corsSettings?.MaxAge > 0)
                    {
                        policy.SetPreflightMaxAge(TimeSpan.FromSeconds(corsSettings.MaxAge));
                    }
                });

                // Production Policy
                options.AddPolicy("ProductionPolicy", policy =>
                {
                    policy.WithOrigins(
                            "https://yourdomain.com",
                            "https://www.yourdomain.com",
                            "https://api.yourdomain.com"
                        )
                        .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")
                        .WithHeaders("Content-Type", "Authorization", "X-Requested-With", "Accept", "Origin")
                        .AllowCredentials();
                });

                // Allow All Policy (for testing only)
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCorsWithConfiguration(
            this IApplicationBuilder app, 
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseCors("DevelopmentPolicy");
            }
            else
            {
                app.UseCors("ProductionPolicy");
            }

            return app;
        }
    }
} 