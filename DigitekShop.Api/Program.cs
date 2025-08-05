using DigitekShop.Application;
using DigitekShop.Application.Features;
using DigitekShop.Infrastructure;
using DigitekShop.Persistence;
using DigitekShop.Api.Extensions;
using DigitekShop.Api.Middleware;
using DigitekShop.Domain.Services;
using DigitekShop.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Ensure the ApplicationServicesRegistration method is defined and accessible
builder.Services.ConfigureAddApplicationServices();

// Add CORS with configuration
builder.Services.AddCorsWithConfiguration(builder.Configuration);
builder.Services.AddFeatures();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.ConfigureIdentityServices(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerDocumentation();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();

// Use CORS with configuration
app.UseCorsWithConfiguration(app.Environment);

// Add CORS logging in development
if (app.Environment.IsDevelopment())
{
    app.UseCorsLogging();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void AddSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(o =>
    {
        o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 1234sddsw'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        o.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

        o.SwaggerDoc("v1", new OpenApiInfo()
        {
            Version = "v1",
            Title = "DigitekShop Api"
        });
    });
}