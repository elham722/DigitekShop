using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.DTOs.Identity
{
    public class UpdatePermissionDto
    {
        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? DisplayName { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(50)]
        public string? Module { get; set; }

        [StringLength(50)]
        public string? Resource { get; set; }

        [StringLength(50)]
        public string? Action { get; set; }

        public bool? IsActive { get; set; }

        public string? UpdatedBy { get; set; }
    }
} 