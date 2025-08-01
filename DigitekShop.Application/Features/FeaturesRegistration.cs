using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Customer;
using DigitekShop.Application.DTOs.Order;
using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Responses;
using DigitekShop.Application.Features.Products.Commands.CreateProduct;
using DigitekShop.Application.Features.Products.Commands.UpdateProduct;
using DigitekShop.Application.Features.Products.Commands.DeleteProduct;
using DigitekShop.Application.Features.Products.Queries.GetProduct;
using DigitekShop.Application.Features.Products.Queries.GetProducts;
using DigitekShop.Application.Features.Customers.Commands.CreateCustomer;
using DigitekShop.Application.Features.Customers.Queries.GetCustomers;
using DigitekShop.Application.Features.Orders.Commands.CreateOrder;
using DigitekShop.Application.Features.Orders.Queries.GetOrders;
using DigitekShop.Application.Services;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Validators.Products;
using DigitekShop.Domain.Interfaces;
using FluentValidation;

namespace DigitekShop.Application.Features
{
    public static class FeaturesRegistration
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            // Register Mediator
            services.AddScoped<IMediator, Mediator>();

            // Register Validation Service
            services.AddScoped<IValidationService, ValidationService>();

            // Register FluentValidation
            services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

            // UnitOfWork is registered in Infrastructure layer

            // Product Commands
            services.AddScoped<ICommandHandler<CreateProductCommand, CommandResponse<ProductDto>>, CreateProductCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateProductCommand, CommandResponse<ProductDto>>, UpdateProductCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteProductCommand>, DeleteProductCommandHandler>();

            // Product Queries
            services.AddScoped<IQueryHandler<GetProductQuery, SuccessResponse<ProductDto>>, GetProductQueryHandler>();
            services.AddScoped<IQueryHandler<GetProductsQuery, PagedResultDto<ProductListDto>>, GetProductsQueryHandler>();

            // Customer Commands
            services.AddScoped<ICommandHandler<CreateCustomerCommand, CustomerDto>, CreateCustomerCommandHandler>();

            // Customer Queries
            services.AddScoped<IQueryHandler<GetCustomersQuery, PagedResultDto<CustomerDto>>, GetCustomersQueryHandler>();

            // Order Commands
            services.AddScoped<ICommandHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();

            // Order Queries
            services.AddScoped<IQueryHandler<GetOrdersQuery, PagedResultDto<OrderDto>>, GetOrdersQueryHandler>();

            return services;
        }
    }
} 