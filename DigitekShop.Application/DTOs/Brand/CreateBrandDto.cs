namespace DigitekShop.Application.DTOs.Brand
{
    public class CreateBrandDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
    }
} 