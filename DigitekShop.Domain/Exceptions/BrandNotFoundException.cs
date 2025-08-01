using System;

namespace DigitekShop.Domain.Exceptions
{
    public class BrandNotFoundException : DomainException
    {
        public int BrandId { get; }

        public BrandNotFoundException(int brandId) 
            : base($"Brand with ID {brandId} was not found.")
        {
            BrandId = brandId;
        }

        public BrandNotFoundException(string brandName) 
            : base($"Brand with name '{brandName}' was not found.")
        {
        }
    }
} 