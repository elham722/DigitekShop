using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Order
{
    public class OrderFilterDto
    {
        public string? OrderNumber { get; set; }
        public int? CustomerId { get; set; }
        public OrderStatus? Status { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? SortBy { get; set; } // "createdAt", "totalAmount", "status"
        public bool SortDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
} 