using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Customer;
using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Features.Customers.Queries.GetCustomers
{
    public record GetCustomersQuery : IQuery<PagedResultDto<CustomerDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? SearchTerm { get; init; }
        public string? Status { get; init; }
        public string? SortBy { get; init; } = "FirstName";
        public bool IsAscending { get; init; } = true;
    }
} 