namespace DigitekShop.Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "IRR";
        public int StockQuantity { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Weight { get; set; }
        public string? Model { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
    }
} 