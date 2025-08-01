namespace DigitekShop.Application.DTOs.Review
{
    public class CreateReviewDto
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Rating { get; set; } // 1-5
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public bool IsVerifiedPurchase { get; set; } = false;
    }
} 