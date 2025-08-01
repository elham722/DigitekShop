namespace DigitekShop.Application.DTOs.Brand
{
    public class UpdateBrandDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
    }
} 