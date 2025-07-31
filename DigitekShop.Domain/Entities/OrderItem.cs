using System;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public Money UnitPrice { get; private set; }
        public Money TotalPrice { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public Money DiscountAmount { get; private set; }
        public string Notes { get; private set; }

        // Navigation Properties
        public Order Order { get; private set; }
        public Product Product { get; private set; }

        // Constructor
        private OrderItem() { }

        public OrderItem(int orderId, int productId, int quantity, Money unitPrice, 
            decimal discountPercentage = 0, string notes = "")
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountPercentage = Math.Max(0, Math.Min(100, discountPercentage)); // بین 0 تا 100
            Notes = notes?.Trim() ?? "";
            
            CalculatePrices();
            SetUpdated();
        }

        // Business Methods
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            Quantity = newQuantity;
            CalculatePrices();
            SetUpdated();
        }

        public void UpdateUnitPrice(Money newPrice)
        {
            UnitPrice = newPrice;
            CalculatePrices();
            SetUpdated();
        }

        public void ApplyDiscount(decimal discountPercentage)
        {
            DiscountPercentage = Math.Max(0, Math.Min(100, discountPercentage));
            CalculatePrices();
            SetUpdated();
        }

        public void RemoveDiscount()
        {
            DiscountPercentage = 0;
            CalculatePrices();
            SetUpdated();
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes?.Trim() ?? "";
            SetUpdated();
        }

        // Business Queries
        public Money GetOriginalTotalPrice()
        {
            return new Money(UnitPrice.Amount * Quantity);
        }
        
        public decimal GetDiscountPercentage() => DiscountPercentage;
        
        public bool HasDiscount() => DiscountPercentage > 0;
        
        public Money GetDiscountAmount() => DiscountAmount;
        
        public Money GetFinalPrice() => TotalPrice;

        private void CalculatePrices()
        {
            // محاسبه قیمت کل بدون تخفیف
            var originalTotal = UnitPrice.Amount * Quantity;
            
            // محاسبه مبلغ تخفیف
            var discountAmount = originalTotal * (DiscountPercentage / 100);
            DiscountAmount = new Money(discountAmount);
            
            // محاسبه قیمت نهایی
            var finalPrice = originalTotal - discountAmount;
            TotalPrice = new Money(Math.Max(0, finalPrice));
        }
    }
} 