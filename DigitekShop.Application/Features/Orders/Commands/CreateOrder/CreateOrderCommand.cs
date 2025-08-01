using DigitekShop.Application.DTOs.Order;
using DigitekShop.Application.Interfaces;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand : ICommand<OrderDto>
    {
        public int CustomerId { get; init; }
        public PaymentMethod PaymentMethod { get; init; }
        public string ShippingMethod { get; init; } = "Standard";
        public string? Notes { get; init; }
        
        // Shipping Address
        public string ShippingStreet { get; init; } = string.Empty;
        public string ShippingCity { get; init; } = string.Empty;
        public string ShippingState { get; init; } = string.Empty;
        public string ShippingPostalCode { get; init; } = string.Empty;
        public string ShippingCountry { get; init; } = string.Empty;
        
        // Billing Address
        public string BillingStreet { get; init; } = string.Empty;
        public string BillingCity { get; init; } = string.Empty;
        public string BillingState { get; init; } = string.Empty;
        public string BillingPostalCode { get; init; } = string.Empty;
        public string BillingCountry { get; init; } = string.Empty;
        
        // Order Items
        public List<CreateOrderItemDto> OrderItems { get; init; } = new();
    }

    public record CreateOrderItemDto
    {
        public int ProductId { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
    }
} 