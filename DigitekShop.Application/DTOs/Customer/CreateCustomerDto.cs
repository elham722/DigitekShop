namespace DigitekShop.Application.DTOs.Customer
{
    public class CreateCustomerDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? NationalCode { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? Notes { get; set; }
    }
} 