using System;
using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.Models.Identity
{
    public class UpdateProfileDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? MiddleName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(20)]
        public string? NationalId { get; set; }

        [StringLength(20)]
        public string? PassportNumber { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        [StringLength(100)]
        public string? Website { get; set; }

        [StringLength(100)]
        public string? Company { get; set; }

        [StringLength(100)]
        public string? JobTitle { get; set; }
    }
} 