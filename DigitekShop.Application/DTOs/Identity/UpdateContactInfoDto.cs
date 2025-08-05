using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.DTOs.Identity
{
    public class UpdateContactInfoDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }
    }
} 