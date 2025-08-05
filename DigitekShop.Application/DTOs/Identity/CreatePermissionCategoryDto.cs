using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.DTOs.Identity
{
    public class CreatePermissionCategoryDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? DisplayName { get; set; }

        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }
    }
} 