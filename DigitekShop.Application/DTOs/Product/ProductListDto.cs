using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Product
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "IRR";
        public int StockQuantity { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public ProductStatus Status { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public bool IsInStock => StockQuantity > 0;
        public bool IsLowStock => StockQuantity <= 10 && StockQuantity > 0;
    }
} 