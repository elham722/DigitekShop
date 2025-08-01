using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public OrderNumber OrderNumber { get; private set; }
        public int CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public Money TotalAmount { get; private set; }
        public Money ShippingCost { get; private set; }
        public Money TaxAmount { get; private set; }
        public Money DiscountAmount { get; private set; }
        public Money FinalAmount { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public string ShippingMethod { get; private set; }
        public Address ShippingAddress { get; private set; }
        public Address BillingAddress { get; private set; }
        public string Notes { get; private set; }
        public DateTime? EstimatedDeliveryDate { get; private set; }
        public DateTime? ShippedAt { get; private set; }
        public DateTime? DeliveredAt { get; private set; }
        public string TrackingNumber { get; private set; }

        // Navigation Properties
        public Customer Customer { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; }

        // Constructor
        private Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(int customerId, Address shippingAddress, Address billingAddress, 
            PaymentMethod paymentMethod, string shippingMethod = "Standard")
        {
            CustomerId = customerId;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            PaymentMethod = paymentMethod;
            ShippingMethod = shippingMethod;
            Status = OrderStatus.Pending;
            
            // تولید شماره سفارش
            OrderNumber = OrderNumber.Generate();
            
            // مقداردهی اولیه
            TotalAmount = new Money(0);
            ShippingCost = new Money(0);
            TaxAmount = new Money(0);
            DiscountAmount = new Money(0);
            FinalAmount = new Money(0);
            
            OrderItems = new List<OrderItem>();
            SetUpdated();
        }

        // Business Methods
        public void AddOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            OrderItems.Add(item);
            RecalculateAmounts();
            SetUpdated();
        }

        public void RemoveOrderItem(int itemId)
        {
            var item = OrderItems.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                OrderItems.Remove(item);
                RecalculateAmounts();
                SetUpdated();
            }
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            
            // ثبت زمان‌های مهم
            if (newStatus == OrderStatus.Shipped && ShippedAt == null)
                ShippedAt = DateTime.UtcNow;
            else if (newStatus == OrderStatus.Delivered && DeliveredAt == null)
                DeliveredAt = DateTime.UtcNow;
                
            SetUpdated();
        }

        public void SetShippingCost(Money cost)
        {
            ShippingCost = cost;
            RecalculateAmounts();
            SetUpdated();
        }

        public void SetTaxAmount(Money tax)
        {
            TaxAmount = tax;
            RecalculateAmounts();
            SetUpdated();
        }

        public void ApplyDiscount(Money discount)
        {
            if (discount.Amount > TotalAmount.Amount)
                throw new ArgumentException("Discount cannot be greater than total amount");

            DiscountAmount = discount;
            RecalculateAmounts();
            SetUpdated();
        }

        public void SetTrackingNumber(string trackingNumber)
        {
            TrackingNumber = trackingNumber?.Trim();
            SetUpdated();
        }

        public void SetEstimatedDeliveryDate(DateTime estimatedDate)
        {
            EstimatedDeliveryDate = estimatedDate;
            SetUpdated();
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes?.Trim() ?? "";
            SetUpdated();
        }

        public void UpdateShippingAddress(Address address)
        {
            ShippingAddress = address;
            SetUpdated();
        }

        public void UpdateBillingAddress(Address address)
        {
            BillingAddress = address;
            SetUpdated();
        }

        // Business Queries
        public int GetItemCount() => OrderItems.Count;
        
        public int GetTotalQuantity()
        {
            return OrderItems.Sum(item => item.Quantity);
        }
        
        public bool IsPending() => Status == OrderStatus.Pending;
        
        public bool IsConfirmed() => Status == OrderStatus.Confirmed;
        
        public bool IsProcessing() => Status == OrderStatus.Processing;
        
        public bool IsShipped() => Status == OrderStatus.Shipped;
        
        public bool IsDelivered() => Status == OrderStatus.Delivered;
        
        public bool IsCancelled() => Status == OrderStatus.Cancelled;
        
        public bool CanBeCancelled()
        {
            return Status == OrderStatus.Pending || Status == OrderStatus.Confirmed;
        }
        
        public bool HasTrackingNumber() => !string.IsNullOrEmpty(TrackingNumber);
        
        public TimeSpan? GetDeliveryTime()
        {
            if (DeliveredAt.HasValue)
                return DeliveredAt.Value - CreatedAt;
            return null;
        }
        
        public bool IsOverdue()
        {
            if (EstimatedDeliveryDate.HasValue)
                return DateTime.UtcNow > EstimatedDeliveryDate.Value && !IsDelivered();
            return false;
        }

        private void RecalculateAmounts()
        {
            // محاسبه مجموع آیتم‌ها
            TotalAmount = new Money(OrderItems.Sum(item => item.TotalPrice.Amount));
            
            // محاسبه مبلغ نهایی
            var finalAmount = TotalAmount.Amount + ShippingCost.Amount + TaxAmount.Amount - DiscountAmount.Amount;
            FinalAmount = new Money(Math.Max(0, finalAmount));
        }
    }
} 