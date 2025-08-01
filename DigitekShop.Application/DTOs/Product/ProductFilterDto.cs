using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Product
{
    public class ProductFilterDto
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public ProductStatus? Status { get; set; }
        public bool? InStock { get; set; }
        public bool? IsExpensive { get; set; }
        public string? SortBy { get; set; } // "name", "price", "createdAt"
        public bool SortDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
} 