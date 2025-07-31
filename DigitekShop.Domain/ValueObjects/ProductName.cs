using System;

namespace DigitekShop.Domain.ValueObjects
{
    public class ProductName
    {
        public string Value { get; private set; }
        
        public ProductName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name cannot be empty");
                
            if (value.Length < 3)
                throw new ArgumentException("Product name must be at least 3 characters");
                
            if (value.Length > 100)
                throw new ArgumentException("Product name cannot exceed 100 characters");
                
            Value = value.Trim();
        }
        
        public static implicit operator string(ProductName productName) => productName.Value;
        
        public static explicit operator ProductName(string value) => new ProductName(value);
        
        public override string ToString() => Value;
        
        public override bool Equals(object obj)
        {
            if (obj is ProductName other)
                return Value.Equals(other.Value);
            return false;
        }
        
        public override int GetHashCode() => Value.GetHashCode();
    }
} 