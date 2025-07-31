using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Exceptions;

namespace DigitekShop.Domain.Services
{
    public class OrderDomainService : IDomainService
    {
        public void ValidateOrderCreation(Customer customer, ICollection<OrderItem> items)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (items == null || !items.Any())
                throw new DomainException("Order must have at least one item");

            if (!customer.IsActive())
                throw new DomainException("Customer must be active to create an order");

            if (customer.IsBlocked())
                throw new DomainException("Blocked customers cannot create orders");

            // بررسی موجودی محصولات
            foreach (var item in items)
            {
                if (!item.Product.IsInStock())
                    throw new DomainException($"Product {item.Product.Name.Value} is out of stock");

                if (item.Quantity > item.Product.StockQuantity)
                    throw new DomainException($"Insufficient stock for product {item.Product.Name.Value}");
            }
        }

        public Money CalculateShippingCost(Address shippingAddress, ICollection<OrderItem> items, string shippingMethod)
        {
            var totalWeight = items.Sum(item => item.Product.Weight * item.Quantity);
            var baseShippingCost = 50000m; // 50,000 تومان هزینه پایه

            // محاسبه بر اساس وزن
            var weightCost = totalWeight * 10000m; // 10,000 تومان به ازای هر کیلو

            // محاسبه بر اساس روش ارسال
            var methodMultiplier = shippingMethod.ToLower() switch
            {
                "express" => 2.0m,
                "premium" => 1.5m,
                _ => 1.0m
            };

            return new Money((baseShippingCost + weightCost) * methodMultiplier);
        }

        public Money CalculateTaxAmount(Money subtotal, string customerType = "individual")
        {
            var taxRate = customerType.ToLower() switch
            {
                "business" => 9.0m, // 9% مالیات برای کسب و کار
                _ => 6.0m // 6% مالیات برای افراد عادی
            };

            return new Money(subtotal.Amount * (taxRate / 100));
        }

        public bool CanApplyDiscount(Order order, Money discountAmount)
        {
            if (order.Status != OrderStatus.Pending)
                return false;

            if (discountAmount.Amount > order.TotalAmount.Amount * 0.3m) // حداکثر 30% تخفیف
                return false;

            return true;
        }

        public bool CanCancelOrder(Order order)
        {
            return order.Status == OrderStatus.Pending || 
                   order.Status == OrderStatus.Confirmed;
        }

        public bool CanRefundOrder(Order order)
        {
            return order.Status == OrderStatus.Delivered && 
                   order.DeliveredAt.HasValue &&
                   DateTime.UtcNow <= order.DeliveredAt.Value.AddDays(7); // حداکثر 7 روز
        }

        public DateTime CalculateEstimatedDeliveryDate(Order order)
        {
            var baseDays = order.ShippingMethod.ToLower() switch
            {
                "express" => 1,
                "premium" => 2,
                _ => 3
            };

            // اضافه کردن روزهای تعطیل
            var weekendDays = CalculateWeekendDays(baseDays);
            var totalDays = baseDays + weekendDays;

            return DateTime.UtcNow.AddDays(totalDays);
        }

        private int CalculateWeekendDays(int baseDays)
        {
            var weekendDays = 0;
            var currentDate = DateTime.UtcNow;

            for (int i = 1; i <= baseDays; i++)
            {
                var checkDate = currentDate.AddDays(i);
                if (checkDate.DayOfWeek == DayOfWeek.Friday || 
                    checkDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    weekendDays++;
                }
            }

            return weekendDays;
        }
    }
} 