using DigitekShop.Application.DTOs.Common;

namespace DigitekShop.Application.DTOs.Wishlist
{
    public class WishlistItemDto : BaseDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSKU { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; }
        public decimal ProductPrice { get; set; }
        public string? Notes { get; set; }
        public DateTime AddedAt { get; set; }
        public bool IsInStock { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerWishlistDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<WishlistItemDto> Items { get; set; } = new();
        public int ItemCount => Items.Count;
    }
} 