using System;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Policies
{
    public class DiscountPolicies
    {
        public class ExpensiveProductDiscountPolicy : IDiscountPolicy
        {
            private readonly decimal _minPrice;
            private readonly decimal _discountPercentage;

            public ExpensiveProductDiscountPolicy(decimal minPrice = 1000000m, decimal discountPercentage = 10)
            {
                _minPrice = minPrice;
                _discountPercentage = discountPercentage;
            }

            public bool IsEligible(Product product)
            {
                return product.Status == ProductStatus.Active &&
                       product.Price.Amount >= _minPrice &&
                       product.StockQuantity > 0;
            }

            public Money CalculateDiscount(Product product)
            {
                if (!IsEligible(product))
                    return new Money(0);

                var discountAmount = product.Price.Amount * (_discountPercentage / 100);
                return new Money(discountAmount);
            }

            public string GetPolicyName() => $"Expensive Product Discount ({_discountPercentage}%)";
        }

        public class LowStockDiscountPolicy : IDiscountPolicy
        {
            private readonly int _threshold;
            private readonly decimal _discountPercentage;

            public LowStockDiscountPolicy(int threshold = 5, decimal discountPercentage = 15)
            {
                _threshold = threshold;
                _discountPercentage = discountPercentage;
            }

            public bool IsEligible(Product product)
            {
                return product.Status == ProductStatus.Active &&
                       product.StockQuantity <= _threshold &&
                       product.StockQuantity > 0;
            }

            public Money CalculateDiscount(Product product)
            {
                if (!IsEligible(product))
                    return new Money(0);

                var discountAmount = product.Price.Amount * (_discountPercentage / 100);
                return new Money(discountAmount);
            }

            public string GetPolicyName() => $"Low Stock Discount ({_discountPercentage}%)";
        }

        public class NewProductDiscountPolicy : IDiscountPolicy
        {
            private readonly int _daysThreshold;
            private readonly decimal _discountPercentage;

            public NewProductDiscountPolicy(int daysThreshold = 30, decimal discountPercentage = 20)
            {
                _daysThreshold = daysThreshold;
                _discountPercentage = discountPercentage;
            }

            public bool IsEligible(Product product)
            {
                if (product.Status != ProductStatus.Active || product.StockQuantity <= 0)
                    return false;

                var daysSinceCreation = (DateTime.UtcNow - product.CreatedAt).Days;
                return daysSinceCreation <= _daysThreshold;
            }

            public Money CalculateDiscount(Product product)
            {
                if (!IsEligible(product))
                    return new Money(0);

                var discountAmount = product.Price.Amount * (_discountPercentage / 100);
                return new Money(discountAmount);
            }

            public string GetPolicyName() => $"New Product Discount ({_discountPercentage}%)";
        }

        public class CategoryBasedDiscountPolicy : IDiscountPolicy
        {
            private readonly int _categoryId;
            private readonly decimal _discountPercentage;

            public CategoryBasedDiscountPolicy(int categoryId, decimal discountPercentage = 12)
            {
                _categoryId = categoryId;
                _discountPercentage = discountPercentage;
            }

            public bool IsEligible(Product product)
            {
                return product.Status == ProductStatus.Active &&
                       product.CategoryId == _categoryId &&
                       product.StockQuantity > 0;
            }

            public Money CalculateDiscount(Product product)
            {
                if (!IsEligible(product))
                    return new Money(0);

                var discountAmount = product.Price.Amount * (_discountPercentage / 100);
                return new Money(discountAmount);
            }

            public string GetPolicyName() => $"Category {_categoryId} Discount ({_discountPercentage}%)";
        }

        public class BrandBasedDiscountPolicy : IDiscountPolicy
        {
            private readonly int _brandId;
            private readonly decimal _discountPercentage;

            public BrandBasedDiscountPolicy(int brandId, decimal discountPercentage = 8)
            {
                _brandId = brandId;
                _discountPercentage = discountPercentage;
            }

            public bool IsEligible(Product product)
            {
                return product.Status == ProductStatus.Active &&
                       product.BrandId == _brandId &&
                       product.StockQuantity > 0;
            }

            public Money CalculateDiscount(Product product)
            {
                if (!IsEligible(product))
                    return new Money(0);

                var discountAmount = product.Price.Amount * (_discountPercentage / 100);
                return new Money(discountAmount);
            }

            public string GetPolicyName() => $"Brand {_brandId} Discount ({_discountPercentage}%)";
        }

        public class SeasonalDiscountPolicy : IDiscountPolicy
        {
            private readonly decimal _discountPercentage;
            private readonly DateTime _startDate;
            private readonly DateTime _endDate;

            public SeasonalDiscountPolicy(decimal discountPercentage, DateTime startDate, DateTime endDate)
            {
                _discountPercentage = discountPercentage;
                _startDate = startDate;
                _endDate = endDate;
            }

            public bool IsEligible(Product product)
            {
                var now = DateTime.UtcNow;
                return product.Status == ProductStatus.Active &&
                       product.StockQuantity > 0 &&
                       now >= _startDate &&
                       now <= _endDate;
            }

            public Money CalculateDiscount(Product product)
            {
                if (!IsEligible(product))
                    return new Money(0);

                var discountAmount = product.Price.Amount * (_discountPercentage / 100);
                return new Money(discountAmount);
            }

            public string GetPolicyName() => $"Seasonal Discount ({_discountPercentage}%)";
        }
    }
} 