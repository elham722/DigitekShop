using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Features.Products.Queries.GetProducts
{
    public record GetProductsQuery : IQuery<PagedResultDto<ProductListDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? SearchTerm { get; init; }
        public int? CategoryId { get; init; }
        public int? BrandId { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public bool? InStockOnly { get; init; }
        public string? SortBy { get; init; } = "Name";
        public bool IsAscending { get; init; } = true;
    }
} 