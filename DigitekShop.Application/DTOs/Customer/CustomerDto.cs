using DigitekShop.Application.DTOs.Common;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Customer
{
    public class CustomerDto : BaseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public DateTime? DateOfBirth { get; set; }
        public string? NationalCode { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public CustomerStatus Status { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Notes { get; set; }
        public int OrderCount { get; set; }
        public int ReviewCount { get; set; }
        public int WishlistCount { get; set; }
        public decimal TotalSpent { get; set; }
        public bool IsActive => Status == CustomerStatus.Active;
        public bool IsBlocked => Status == CustomerStatus.Blocked;
    }
} 