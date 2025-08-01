using DigitekShop.Domain.Enums;
using DigitekShop.Application.DTOs.Common;

namespace DigitekShop.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public AddressDto ShippingAddress { get; set; } = new();
        public AddressDto BillingAddress { get; set; } = new();
        public PaymentMethod PaymentMethod { get; set; }
        public string ShippingMethod { get; set; } = "Standard";
        public string? Notes { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class AddressDto
    {
        public string Province { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? Alley { get; set; }
        public string? Building { get; set; }
        public string? Unit { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }
    }
} 