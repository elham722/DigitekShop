using DigitekShop.Application;
using DigitekShop.Application.Features;
using DigitekShop.Infrastructure;
using DigitekShop.Persistence;
using DigitekShop.Api.Extensions;
using DigitekShop.Api.Middleware;

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
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use CORS with configuration
app.UseCorsWithConfiguration(app.Environment);

// Add CORS logging in development
if (app.Environment.IsDevelopment())
{
    app.UseCorsLogging();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
