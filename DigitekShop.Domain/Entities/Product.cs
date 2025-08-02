using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Entities
{
    public class Product : BaseEntity
    {
        public ProductName Name { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public int StockQuantity { get; private set; }
        public SKU SKU { get; private set; }
        public string ImageUrl { get; private set; }
        public ProductStatus Status { get; private set; }
        public decimal Weight { get; private set; }
        public string Model { get; private set; }

        // Navigation Properties
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int? BrandId { get; private set; }
        public Brand Brand { get; private set; }
        public ICollection<Review> Reviews { get; private set; } = new List<Review>();

        // Constructor
        private Product() { } // برای EF Core

        public Product(ProductName name, string description, Money price, int stockQuantity,
            SKU sku, int categoryId, int? brandId = null, string model = "", decimal weight = 0)
        {
            Name = name;
            Description = description ?? "";
            Price = price;
            StockQuantity = stockQuantity;
            SKU = sku;
            CategoryId = categoryId;
            BrandId = brandId;
            Model = model ?? "";
            Weight = weight;
            Status = ProductStatus.Active;
            SetUpdated();
        }

        // Business Methods
        public void UpdateStock(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative");

            StockQuantity = newQuantity;
            UpdateStatus();
            SetUpdated();
        }

        public void UpdatePrice(Money newPrice)
        {
            Price = newPrice;
            SetUpdated();
        }

        public void UpdateName(ProductName newName)
        {
            Name = newName;
            SetUpdated();
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? "";
            SetUpdated();
        }

        public void UpdateImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            SetUpdated();
        }

        public void Deactivate()
        {
            Status = ProductStatus.Inactive;
            SetUpdated();
        }

        public void Activate()
        {
            Status = ProductStatus.Active;
            SetUpdated();
        }

        public void Discontinue()
        {
            Status = ProductStatus.Discontinued;
            SetUpdated();
        }

        public void MarkAsComingSoon()
        {
            Status = ProductStatus.ComingSoon;
            SetUpdated();
        }

        public void ApplyDiscount(decimal discountPercentage)
        {
            Price = Price.ApplyDiscount(discountPercentage);
            SetUpdated();
        }

        public void ApplyTax(decimal taxRate)
        {
            Price = Price.ApplyTax(taxRate);
            SetUpdated();
        }

        public bool IsInStock() => StockQuantity > 0 && Status == ProductStatus.Active;
        
        public bool IsExpensive() => Price.IsExpensive();
        
        public bool IsLowStock(int threshold = 10) => StockQuantity <= threshold && StockQuantity > 0;

        private void UpdateStatus()
        {
            if (StockQuantity == 0 && Status == ProductStatus.Active)
            {
                Status = ProductStatus.OutOfStock;
            }
            else if (StockQuantity > 0 && Status == ProductStatus.OutOfStock)
            {
                Status = ProductStatus.Active;
            }
        }

        public string GetDisplayName() => Name.Value;
        
        public string GetFullName() => $"{Brand?.Name.Value ?? ""} {Model}".Trim();

        public decimal GetAverageRating()
        {
            if (Reviews == null || !Reviews.Any())
                return 0;

            var approvedReviews = Reviews.Where(r => r.IsApproved).ToList();
            if (!approvedReviews.Any())
                return 0;

            return (decimal)Math.Round(approvedReviews.Average(r => r.Rating), 1);
        }

        public int GetReviewCount()
        {
            return Reviews?.Count(r => r.IsApproved) ?? 0;
        }

        public IEnumerable<Review> GetApprovedReviews()
        {
            return Reviews?.Where(r => r.IsApproved).OrderByDescending(r => r.CreatedAt) ?? Enumerable.Empty<Review>();
        }
    }
}
