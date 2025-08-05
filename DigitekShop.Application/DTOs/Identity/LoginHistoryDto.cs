using System;

namespace DigitekShop.Application.DTOs.Identity
{
    public class LoginHistoryDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public DateTime LoginAt { get; set; }
        public DateTime? LogoutAt { get; set; }
        public bool IsSuccessful { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Location { get; set; }
        public string? Browser { get; set; }
        public string? OperatingSystem { get; set; }
        public string? DeviceType { get; set; }
        public string? FailureReason { get; set; }
        public bool IsTwoFactor { get; set; }
        public string? SessionId { get; set; }
    }
} 