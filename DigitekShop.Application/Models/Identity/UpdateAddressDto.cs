using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.Models.Identity
{
    public class UpdateAddressDto
    {
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
    }
} 