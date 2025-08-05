using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DigitekShop.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Personal Information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastPasswordChangeAt { get; set; }
        public int LoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        // Preferences
        public string? PreferredLanguage { get; set; } = "fa-IR";
        public string? TimeZone { get; set; } = "Asia/Tehran";
        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = false;
        public bool PushNotifications { get; set; } = true;

        // Security
        public bool TwoFactorEnabled { get; set; } = false;
        public string? TwoFactorSecret { get; set; }
        public DateTime? TwoFactorEnabledAt { get; set; }

        // Integration
        public int? CustomerId { get; set; } // Link to Domain Customer entity
        public string? CustomerNumber { get; set; }

        // Audit
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();
        public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();

        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FullName) ? FullName : UserName ?? Email ?? "";
        public int Age => DateOfBirth.HasValue ? DateTime.UtcNow.Year - DateOfBirth.Value.Year : 0;
        public bool IsLocked => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
        public bool IsNewUser => DateTime.UtcNow.Subtract(CreatedAt).Days <= 7;
        public bool HasProfilePicture => !string.IsNullOrEmpty(ProfilePictureUrl);

        // Business Methods
        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
            LoginAttempts = 0;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncrementLoginAttempts()
        {
            LoginAttempts++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ResetLoginAttempts()
        {
            LoginAttempts = 0;
            UpdatedAt = DateTime.UtcNow;
        }

        public void LockAccount(TimeSpan duration)
        {
            LockoutEnd = DateTime.UtcNow.Add(duration);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UnlockAccount()
        {
            LockoutEnd = null;
            LoginAttempts = 0;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePassword()
        {
            LastPasswordChangeAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void EnableTwoFactor()
        {
            TwoFactorEnabled = true;
            TwoFactorEnabledAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DisableTwoFactor()
        {
            TwoFactorEnabled = false;
            TwoFactorSecret = null;
            TwoFactorEnabledAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateProfile(string firstName, string lastName, string? middleName = null)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContactInfo(string email, string? phoneNumber = null)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAddress(string? address, string? city, string? state, string? country, string? postalCode)
        {
            Address = address;
            City = city;
            State = state;
            Country = country;
            PostalCode = postalCode;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePreferences(string? preferredLanguage, string? timeZone, bool emailNotifications, bool smsNotifications, bool pushNotifications)
        {
            PreferredLanguage = preferredLanguage;
            TimeZone = timeZone;
            EmailNotifications = emailNotifications;
            SmsNotifications = smsNotifications;
            PushNotifications = pushNotifications;
            UpdatedAt = DateTime.UtcNow;
        }

        // Validation Methods
        public bool IsValidForRegistration()
        {
            return !string.IsNullOrEmpty(Email) && 
                   !string.IsNullOrEmpty(UserName) && 
                   !string.IsNullOrEmpty(FirstName) && 
                   !string.IsNullOrEmpty(LastName);
        }

        public bool IsValidForLogin()
        {
            return IsActive && !IsDeleted && !IsLocked;
        }

        public bool RequiresPasswordChange()
        {
            return LastPasswordChangeAt.HasValue && 
                   DateTime.UtcNow.Subtract(LastPasswordChangeAt.Value).Days > 90;
        }

        public bool IsPasswordExpired()
        {
            return LastPasswordChangeAt.HasValue && 
                   DateTime.UtcNow.Subtract(LastPasswordChangeAt.Value).Days > 180;
        }
    }
}
