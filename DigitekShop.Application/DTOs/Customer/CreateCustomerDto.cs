namespace DigitekShop.Application.DTOs.Customer
{
    public class CreateCustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? NationalCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Notes { get; set; }
        public string? Street { get; set; } // Added missing property  
        public string? City { get; set; } // Added missing property  
        public string? State { get; set; } // Added missing property  
        public string? PostalCode { get; set; } // Added missing property  
        public string? Country { get; set; } // Added missing property  
    }
} 