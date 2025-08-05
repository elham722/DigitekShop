using System;

namespace DigitekShop.Application.Models.Identity
{
    public class UserLoginHistoryDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Location { get; set; }
        public bool IsSuccessful { get; set; }
        public string? FailureReason { get; set; }
        public string? SessionId { get; set; }
        public TimeSpan? SessionDuration { get; set; }
    }
} 