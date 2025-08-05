using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.Models.Identity
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