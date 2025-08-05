using System;
using System.Collections.Generic;

namespace DigitekShop.Application.DTOs.Identity
{
    public class UserDto
    {
        // Basic Information
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        // Personal Information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }

        // Account Information
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastPasswordChangeAt { get; set; }
        public int LoginAttempts { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        // Security
        public bool TwoFactorEnabled { get; set; }
        public DateTime? TwoFactorEnabledAt { get; set; }

        // Integration with Domain
        public int? CustomerId { get; set; }

        // Audit
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Computed Properties
        public bool IsLocked { get; set; }
        public bool IsNewUser { get; set; }
        public bool RequiresPasswordChange { get; set; }
        public bool IsPasswordExpired { get; set; }

        // Roles and Permissions
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<string> Permissions { get; set; } = new List<string>();

        // Activity Information
        public int TotalLogins { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public string? LastIpAddress { get; set; }
        public string? LastUserAgent { get; set; }
    }
} 