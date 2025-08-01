using DigitekShop.Application.DTOs.Common;

namespace DigitekShop.Application.DTOs.Brand
{
    public class BrandDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
        public bool IsPopular => ProductCount > 10;
    }
} 