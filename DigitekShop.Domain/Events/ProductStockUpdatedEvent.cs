using System;

namespace DigitekShop.Domain.Events
{
    public class ProductStockUpdatedEvent : IDomainEvent
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public string SKU { get; }
        public int OldQuantity { get; }
        public int NewQuantity { get; }
        public int ChangeAmount { get; }
        public DateTime OccurredOn { get; }

        public ProductStockUpdatedEvent(int productId, string productName, string sku, 
            int oldQuantity, int newQuantity)
        {
            ProductId = productId;
            ProductName = productName;
            SKU = sku;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
            ChangeAmount = newQuantity - oldQuantity;
            OccurredOn = DateTime.UtcNow;
        }
    }
} 