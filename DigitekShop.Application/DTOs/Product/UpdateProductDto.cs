namespace DigitekShop.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Weight { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
    }
} 