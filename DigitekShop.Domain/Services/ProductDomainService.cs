using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Exceptions;

namespace DigitekShop.Domain.Services
{
    public class ProductDomainService : IDomainService
    {
        public void ValidateProductCreation(ProductName name, Money price, int stockQuantity, int categoryId)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (price == null)
                throw new ArgumentNullException(nameof(price));

            if (price.Amount <= 0)
                throw new DomainException("Product price must be greater than zero");

            if (stockQuantity < 0)
                throw new DomainException("Stock quantity cannot be negative");

            if (categoryId <= 0)
                throw new DomainException("Category ID must be valid");
        }

        public bool IsProductEligibleForDiscount(Product product, decimal discountPercentage)
        {
            if (product == null)
                return false;

            if (product.Status != ProductStatus.Active)
                return false;

            if (discountPercentage <= 0 || discountPercentage > 50) // حداکثر 50% تخفیف
                return false;

            if (product.Price.Amount < 100000) // محصولات زیر 100 هزار تومان تخفیف نمی‌گیرند
                return false;

            return true;
        }

        public bool ShouldMarkAsLowStock(Product product, int threshold = 10)
        {
            return product.StockQuantity <= threshold && product.StockQuantity > 0;
        }

        public bool ShouldMarkAsOutOfStock(Product product)
        {
            return product.StockQuantity == 0 && product.Status == ProductStatus.Active;
        }

        public bool CanUpdateStock(Product product, int newQuantity)
        {
            if (product == null)
                return false;

            if (newQuantity < 0)
                return false;

            return true;
        }

        public Money CalculateDiscountedPrice(Product product, decimal discountPercentage)
        {
            if (!IsProductEligibleForDiscount(product, discountPercentage))
                throw new DomainException("Product is not eligible for discount");

            return product.Price.ApplyDiscount(discountPercentage);
        }

        public bool IsProductExpensive(Product product, decimal threshold = 1000000m)
        {
            return product.Price.Amount > threshold;
        }

        public bool IsProductAffordable(Product product, decimal customerBudget)
        {
            return product.Price.Amount <= customerBudget;
        }

        public bool ShouldShowComingSoon(Product product)
        {
            return product.Status == ProductStatus.ComingSoon && 
                   product.StockQuantity == 0;
        }

        public bool IsProductDiscontinued(Product product)
        {
            return product.Status == ProductStatus.Discontinued;
        }

        public bool CanReactivateProduct(Product product)
        {
            return product.Status == ProductStatus.Discontinued && 
                   product.StockQuantity > 0;
        }

        public string GetProductStatusDescription(Product product)
        {
            return product.Status switch
            {
                ProductStatus.Active => "فعال",
                ProductStatus.Inactive => "غیرفعال",
                ProductStatus.OutOfStock => "ناموجود",
                ProductStatus.ComingSoon => "به زودی",
                ProductStatus.Discontinued => "متوقف شده",
                _ => "نامشخص"
            };
        }

        public bool IsProductInSameCategory(Product product1, Product product2)
        {
            return product1.CategoryId == product2.CategoryId;
        }

        public bool IsProductFromSameBrand(Product product1, Product product2)
        {
            return product1.BrandId.HasValue && 
                   product2.BrandId.HasValue && 
                   product1.BrandId == product2.BrandId;
        }
    }
} 