using DigitekShop.Application.DTOs.Common;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Order
{
    public class OrderDto : BaseDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ShippingMethod { get; set; } = string.Empty;
        public AddressDto ShippingAddress { get; set; } = new();
        public AddressDto BillingAddress { get; set; } = new();
        public string? Notes { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string? TrackingNumber { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
        public int ItemCount => OrderItems.Count;
        public bool IsDelivered => Status == OrderStatus.Delivered;
        public bool CanBeCancelled => Status == OrderStatus.Pending || Status == OrderStatus.Confirmed;
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSKU { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
} 