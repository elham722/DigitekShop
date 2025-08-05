using Microsoft.AspNetCore.Identity;

namespace DigitekShop.Identity.Models
{
    public class UserToken : IdentityUserToken<string>
    {
        // Additional properties can be added here if needed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? ExpiresAt { get; set; }
        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedBy { get; set; }
        public string? RevocationReason { get; set; }
    }
} 