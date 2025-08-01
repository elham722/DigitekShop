using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Order;
using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Features.Orders.Queries.GetOrders
{
    public record GetOrdersQuery : IQuery<PagedResultDto<OrderDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public int? CustomerId { get; init; }
        public string? Status { get; init; }
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }
        public string? SortBy { get; init; } = "CreatedAt";
        public bool IsAscending { get; init; } = false; // جدیدترین سفارش‌ها اول
    }
} 