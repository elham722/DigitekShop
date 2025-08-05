using System;
using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class ProductStockUpdatedEvent : BaseDomainEvent
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public string SKU { get; }
        public int OldStockQuantity { get; }
        public int NewStockQuantity { get; }
        public int StockChange { get; }
        public string Reason { get; }
        public string ChangedBy { get; }

        public ProductStockUpdatedEvent(Product product, int oldStockQuantity, int newStockQuantity, string reason = "", string changedBy = "System")
            : base("Product", product.Id.ToString())
        {
            ProductId = product.Id;
            ProductName = product.Name.Value;
            SKU = product.SKU.Value;
            OldStockQuantity = oldStockQuantity;
            NewStockQuantity = newStockQuantity;
            StockChange = newStockQuantity - oldStockQuantity;
            Reason = reason;
            ChangedBy = changedBy;
        }
    }
} 