using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Policies;

namespace DigitekShop.Domain.Services
{
    public class DiscountCalculatorService : IDomainService
    {
        private readonly List<IDiscountPolicy> _policies;

        public DiscountCalculatorService()
        {
            _policies = new List<IDiscountPolicy>();
        }

        public DiscountCalculatorService(IEnumerable<IDiscountPolicy> policies)
        {
            _policies = policies?.ToList() ?? new List<IDiscountPolicy>();
        }

        public void AddPolicy(IDiscountPolicy policy)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            _policies.Add(policy);
        }

        public void AddPolicies(IEnumerable<IDiscountPolicy> policies)
        {
            if (policies == null)
                throw new ArgumentNullException(nameof(policies));

            _policies.AddRange(policies);
        }

        public Money CalculateBestDiscount(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var applicablePolicies = _policies.Where(p => p.IsEligible(product)).ToList();

            if (!applicablePolicies.Any())
                return new Money(0);

            var bestDiscount = applicablePolicies
                .Select(policy => policy.CalculateDiscount(product))
                .OrderByDescending(discount => discount.Amount)
                .First();

            return bestDiscount;
        }

        public List<DiscountInfo> GetApplicableDiscounts(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            return _policies
                .Where(p => p.IsEligible(product))
                .Select(p => new DiscountInfo
                {
                    PolicyName = p.GetPolicyName(),
                    DiscountAmount = p.CalculateDiscount(product),
                    DiscountPercentage = CalculateDiscountPercentage(product.Price, p.CalculateDiscount(product))
                })
                .OrderByDescending(d => d.DiscountAmount.Amount)
                .ToList();
        }

        public Money CalculateTotalDiscountForOrder(IEnumerable<OrderItem> orderItems)
        {
            if (orderItems == null)
                throw new ArgumentNullException(nameof(orderItems));

            var totalDiscount = new Money(0);

            foreach (var item in orderItems)
            {
                var itemDiscount = CalculateBestDiscount(item.Product);
                var totalItemDiscount = new Money(itemDiscount.Amount * item.Quantity);
                totalDiscount = totalDiscount.Add(totalItemDiscount);
            }

            return totalDiscount;
        }

        public List<OrderItemDiscountInfo> GetOrderItemDiscounts(IEnumerable<OrderItem> orderItems)
        {
            if (orderItems == null)
                throw new ArgumentNullException(nameof(orderItems));

            var result = new List<OrderItemDiscountInfo>();

            foreach (var item in orderItems)
            {
                var applicableDiscounts = GetApplicableDiscounts(item.Product);
                var bestDiscount = applicableDiscounts.FirstOrDefault();

                result.Add(new OrderItemDiscountInfo
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name.Value,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                    BestDiscount = bestDiscount?.DiscountAmount ?? new Money(0),
                    BestDiscountPercentage = bestDiscount?.DiscountPercentage ?? 0,
                    AllDiscounts = applicableDiscounts
                });
            }

            return result;
        }

        private decimal CalculateDiscountPercentage(Money originalPrice, Money discountAmount)
        {
            if (originalPrice.Amount == 0)
                return 0;

            return (discountAmount.Amount / originalPrice.Amount) * 100;
        }

        public void ClearPolicies()
        {
            _policies.Clear();
        }

        public int GetPolicyCount()
        {
            return _policies.Count;
        }
    }

    public class DiscountInfo
    {
        public string PolicyName { get; set; }
        public Money DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
    }

    public class OrderItemDiscountInfo
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public Money UnitPrice { get; set; }
        public Money TotalPrice { get; set; }
        public Money BestDiscount { get; set; }
        public decimal BestDiscountPercentage { get; set; }
        public List<DiscountInfo> AllDiscounts { get; set; }
    }
} 