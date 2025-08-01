using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Category
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public CategoryType? Type { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ImageUrl { get; set; }
    }
} 