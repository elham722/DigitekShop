using System;
using System.Collections.Generic;

namespace DigitekShop.Application.DTOs.Identity
{
    public class AuthResponseDto
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<string> Permissions { get; set; } = new List<string>();
        public bool RequiresTwoFactor { get; set; }
        public string? TwoFactorToken { get; set; }
        public bool IsLocked { get; set; }
        public bool RequiresPasswordChange { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
} 