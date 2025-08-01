namespace DigitekShop.Application.DTOs.Wishlist
{
    public class CreateWishlistItemDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateWishlistItemDto
    {
        public int Id { get; set; }
        public string? Notes { get; set; }
    }
} 