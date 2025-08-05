using System;
using System.Collections.Generic;

namespace DigitekShop.Application.Models.Identity
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
        public string? NationalId { get; set; }
        public string? PassportNumber { get; set; }

        // Address Information
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }

        // Profile Information
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        public string? Website { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }

        // Account Information
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastPasswordChangeAt { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        // Preferences
        public string? PreferredLanguage { get; set; }
        public string? TimeZone { get; set; }
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool PushNotifications { get; set; }

        // Security
        public bool TwoFactorEnabled { get; set; }
        public DateTime? TwoFactorEnabledAt { get; set; }

        // Integration
        public int? CustomerId { get; set; }
        public string? CustomerNumber { get; set; }

        // Audit
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Computed Properties
        public bool IsLocked { get; set; }
        public bool IsNewUser { get; set; }
        public bool HasProfilePicture { get; set; }
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