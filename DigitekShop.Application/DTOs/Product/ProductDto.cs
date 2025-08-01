using DigitekShop.Application.DTOs.Common;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Product
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "IRR";
        public int StockQuantity { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public ProductStatus Status { get; set; }
        public decimal Weight { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int? BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public bool IsInStock => StockQuantity > 0;
        public bool IsLowStock => StockQuantity <= 10 && StockQuantity > 0;
        public bool IsExpensive => Price > 10000000; // 10 million IRR
    }
} 