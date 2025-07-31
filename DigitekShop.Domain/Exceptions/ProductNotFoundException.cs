using System;

namespace DigitekShop.Domain.Exceptions
{
    public class ProductNotFoundException : DomainException
    {
        public int ProductId { get; }

        public ProductNotFoundException(int productId) 
            : base($"Product with ID {productId} was not found.")
        {
            ProductId = productId;
        }

        public ProductNotFoundException(string sku) 
            : base($"Product with SKU {sku} was not found.")
        {
        }
    }
} 