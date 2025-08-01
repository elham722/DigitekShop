using System;

namespace DigitekShop.Domain.Exceptions
{
    public class InvalidProductDataException : DomainException
    {
        public string PropertyName { get; }

        public InvalidProductDataException(string propertyName, string message) 
            : base($"Invalid product data for {propertyName}: {message}")
        {
            PropertyName = propertyName;
        }

        public InvalidProductDataException(string message) 
            : base($"Invalid product data: {message}")
        {
        }
    }
} 