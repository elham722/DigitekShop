
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DigitekShop.Application.Interfaces.Identity;
using DigitekShop.Identity.Models;
using DigitekShop.Identity.Services;
using DigitekShop.Identity.Context;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Identity
{
    public static class IdentityServicesRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Specify assemblies to scan for AutoMapper profiles
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));


            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddDbContext<DigitekShopIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DigitekIdentityDBConnection"),
                    b=>b.MigrationsAssembly(typeof(DigitekShopIdentityDbContext).Assembly.FullName)
                    );
            });

            services.AddIdentity<ApplicationUser, IdentityRole<string>>()
                .AddEntityFrameworkStores<DigitekShopIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IPermissionService, PermissionService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                    };
                });

            return services;
        }
    }
}
