using System;

namespace DigitekShop.Domain.Exceptions
{
    public class InsufficientStockException : DomainException
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        public InsufficientStockException(int productId, string productName, int requestedQuantity, int availableQuantity)
            : base($"Insufficient stock for product {productName}. Requested: {requestedQuantity}, Available: {availableQuantity}")
        {
            ProductId = productId;
            ProductName = productName;
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }
} 