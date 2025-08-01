using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Order
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal? ShippingCost { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string? Notes { get; set; }
        public AddressDto? ShippingAddress { get; set; }
        public AddressDto? BillingAddress { get; set; }
    }
} 