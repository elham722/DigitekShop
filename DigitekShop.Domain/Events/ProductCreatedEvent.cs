using System;
using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class ProductCreatedEvent : IDomainEvent
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public string SKU { get; }
        public decimal Price { get; }
        public int CategoryId { get; }
        public int? BrandId { get; }
        public DateTime OccurredOn { get; }

        public ProductCreatedEvent(Product product)
        {
            ProductId = product.Id;
            ProductName = product.Name.Value;
            SKU = product.SKU.Value;
            Price = product.Price.Amount;
            CategoryId = product.CategoryId;
            BrandId = product.BrandId;
            OccurredOn = DateTime.UtcNow;
        }
    }
} 