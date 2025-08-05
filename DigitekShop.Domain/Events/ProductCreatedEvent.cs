using System;
using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class ProductCreatedEvent : BaseDomainEvent
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public string SKU { get; }
        public decimal Price { get; }
        public int StockQuantity { get; }
        public int CategoryId { get; }
        public int? BrandId { get; }
        public string ProductStatus { get; }

        public ProductCreatedEvent(Product product)
            : base("Product", product.Id.ToString())
        {
            ProductId = product.Id;
            ProductName = product.Name.Value;
            SKU = product.SKU.Value;
            Price = product.Price.Amount;
            StockQuantity = product.StockQuantity;
            CategoryId = product.CategoryId;
            BrandId = product.BrandId;
            ProductStatus = product.Status.ToString();
        }
    }
} 