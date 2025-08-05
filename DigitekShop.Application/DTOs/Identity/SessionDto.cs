using System;

namespace DigitekShop.Application.DTOs.Identity
{
    public class SessionDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? DeviceName { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }
        public string? Location { get; set; }
        public string? Browser { get; set; }
        public string? OperatingSystem { get; set; }
    }
} 