using System;
using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.Models.Identity
{
    public class RegisterUserDto
    {
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
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? MiddleName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(20)]
        public string? NationalId { get; set; }

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

        // Terms and Conditions
        public bool AcceptTerms { get; set; }

        public bool AcceptMarketing { get; set; }

        // Integration
        public int? CustomerId { get; set; }

        public string? CustomerNumber { get; set; }
    }
} 