using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.DTOs.Identity
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public IEnumerable<string> Permissions { get; set; } = new List<string>();

        public string? CreatedBy { get; set; }
    }
} 