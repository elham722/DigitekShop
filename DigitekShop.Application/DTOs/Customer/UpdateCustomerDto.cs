namespace DigitekShop.Application.DTOs.Customer
{
    public class UpdateCustomerDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Notes { get; set; }
    }
} 