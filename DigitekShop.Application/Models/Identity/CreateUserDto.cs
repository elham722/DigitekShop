using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.Models.Identity
{
    public class CreateUserDto
    {
        // Required Information
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        // Optional Personal Information
        [StringLength(50)]
        public string? MiddleName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(20)]
        public string? NationalId { get; set; }

        [StringLength(20)]
        public string? PassportNumber { get; set; }

        // Contact Information
        [Phone]
        public string? PhoneNumber { get; set; }

        // Address Information
        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        // Profile Information
        [StringLength(500)]
        public string? Bio { get; set; }

        [StringLength(100)]
        public string? Website { get; set; }

        [StringLength(100)]
        public string? Company { get; set; }

        [StringLength(100)]
        public string? JobTitle { get; set; }

        // Preferences
        [StringLength(10)]
        public string? PreferredLanguage { get; set; } = "fa-IR";

        [StringLength(50)]
        public string? TimeZone { get; set; } = "Asia/Tehran";

        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = false;
        public bool PushNotifications { get; set; } = true;

        // Security
        public bool EmailConfirmed { get; set; } = false;
        public bool PhoneNumberConfirmed { get; set; } = false;
        public bool TwoFactorEnabled { get; set; } = false;

        // Integration
        public int? CustomerId { get; set; }
        public string? CustomerNumber { get; set; }

        // Roles
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        // Audit
        public string? CreatedBy { get; set; }
    }
} 