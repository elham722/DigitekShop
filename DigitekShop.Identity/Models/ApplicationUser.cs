using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DigitekShop.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Basic Identity Information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        // Account Information
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastPasswordChangeAt { get; set; }
        public int LoginAttempts { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        // Security
        public bool TwoFactorEnabled { get; set; } = false;
        public string? TwoFactorSecret { get; set; }
        public DateTime? TwoFactorEnabledAt { get; set; }

        // Integration with Domain
        public int? CustomerId { get; set; } // Optional link to Domain Customer entity

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

        // Identity Business Methods
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

        public bool IsValidForRegistration()
        {
            return !string.IsNullOrEmpty(Email) && 
                   !string.IsNullOrEmpty(UserName) && 
                   !IsDeleted;
        }

        public bool IsValidForLogin()
        {
            return IsActive && !IsDeleted && !IsLocked;
        }

        public bool RequiresPasswordChange()
        {
            return LastPasswordChangeAt == null || 
                   DateTime.UtcNow.Subtract(LastPasswordChangeAt.Value).Days > 90;
        }

        public bool IsPasswordExpired()
        {
            return LastPasswordChangeAt != null && 
                   DateTime.UtcNow.Subtract(LastPasswordChangeAt.Value).Days > 365;
        }

        public void LinkToCustomer(int customerId)
        {
            CustomerId = customerId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UnlinkFromCustomer()
        {
            CustomerId = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
