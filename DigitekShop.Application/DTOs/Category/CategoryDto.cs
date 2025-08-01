using DigitekShop.Application.DTOs.Common;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Category
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CategoryType Type { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
        public List<CategoryDto> SubCategories { get; set; } = new();
        public bool HasSubCategories => SubCategories.Any();
        public bool IsMainCategory => ParentCategoryId == null;
    }
} 