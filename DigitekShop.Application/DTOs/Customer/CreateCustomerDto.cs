using System;
using System.ComponentModel.DataAnnotations;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Customer
{
    public class CreateCustomerDto
    {
        // Required Information
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        // Optional Personal Information
        [StringLength(50)]
        public string? MiddleName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(20)]
        public string? NationalCode { get; set; }

        [StringLength(20)]
        public string? PassportNumber { get; set; }

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

        // Business Information
        public CustomerType Type { get; set; } = CustomerType.Regular;

        // Integration with Identity
        public string? UserId { get; set; }

        // Notes
        [StringLength(1000)]
        public string? Notes { get; set; }
    }
} 