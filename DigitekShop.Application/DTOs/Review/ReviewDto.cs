using DigitekShop.Application.DTOs.Common;

namespace DigitekShop.Application.DTOs.Review
{
    public class ReviewDto : BaseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public bool IsVerifiedPurchase { get; set; }
        public bool IsHelpful { get; set; }
        public int HelpfulCount { get; set; }
        public bool IsApproved { get; set; }
        public string? AdminResponse { get; set; }
    }
} 