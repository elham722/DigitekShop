using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CategoryType Type { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ImageUrl { get; set; }
    }
} 