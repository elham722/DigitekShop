using System;

namespace DigitekShop.Application.DTOs.Identity
{
    public class PermissionActivityDto
    {
        public string Id { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? TargetRole { get; set; }
        public string? TargetUser { get; set; }
    }
} 