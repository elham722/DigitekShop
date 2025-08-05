using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.DTOs.Identity
{
    public class UpdateRoleDto
    {
        [StringLength(50, MinimumLength = 2)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        public IEnumerable<string>? Permissions { get; set; }

        public string? UpdatedBy { get; set; }
    }
} 