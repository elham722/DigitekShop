using System;

namespace DigitekShop.Application.DTOs.Identity
{
    public class RoleActivityDto
    {
        public string Id { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
} 