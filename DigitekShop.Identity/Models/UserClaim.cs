using Microsoft.AspNetCore.Identity;

namespace DigitekShop.Identity.Models
{
    public class UserClaim : IdentityUserClaim<string>
    {
        // Additional properties can be added here if needed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 