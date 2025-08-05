using DigitekShop.Application.DTOs.Common;
using DigitekShop.Domain.Enums;
using System;

namespace DigitekShop.Application.DTOs.Customer
{
    public class CustomerDto : BaseDto
    {
        // Business Information
        public string CustomerNumber { get; set; } = string.Empty;
        public CustomerStatus Status { get; set; }
        public CustomerType Type { get; set; }

        // Personal Information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FullName) ? FullName : Email;
        public DateTime? DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? NationalCode { get; set; }
        public string? PassportNumber { get; set; }

        // Contact Information
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }

        // Profile Information
        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }
        public string? Website { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }

        // Business Data
        public decimal TotalSpent { get; set; }
        public int TotalOrders { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // Verification
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }

        // Integration with Identity
        public string? UserId { get; set; }

        // Preferences
        public string? PreferredLanguage { get; set; }
        public string? TimeZone { get; set; }
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool PushNotifications { get; set; }

        // Notes and Audit
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Computed Properties
        public bool IsActive => Status == CustomerStatus.Active;
        public bool IsBlocked => Status == CustomerStatus.Blocked;
        public bool IsVerified => IsEmailVerified && IsPhoneVerified;
        public bool IsLinkedToUser => !string.IsNullOrEmpty(UserId);
        public bool IsNewCustomer { get; set; }
        public bool IsVipCustomer { get; set; }

        // Statistics
        public int OrderCount { get; set; }
        public int ReviewCount { get; set; }
        public int WishlistCount { get; set; }
    }
} 