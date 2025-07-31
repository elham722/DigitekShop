using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Events;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Exceptions;

namespace DigitekShop.Domain.Aggregates
{
    public class OrderAggregate
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        
        public Order Order { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => Order.OrderItems.ToList().AsReadOnly();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public OrderAggregate(Order order)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
        }

        public static OrderAggregate Create(int customerId, Address shippingAddress, Address billingAddress, 
            PaymentMethod paymentMethod, string shippingMethod = "Standard")
        {
            var order = new Order(customerId, shippingAddress, billingAddress, paymentMethod, shippingMethod);
            var aggregate = new OrderAggregate(order);
            
            // اضافه کردن event
            aggregate.AddDomainEvent(new OrderCreatedEvent(order));
            
            return aggregate;
        }

        public void AddOrderItem(Product product, int quantity, Money unitPrice)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            if (unitPrice == null)
                throw new ArgumentNullException(nameof(unitPrice));

            // بررسی موجودی
            if (!product.IsInStock())
                throw new InsufficientStockException(product.Id, product.Name.Value, quantity, product.StockQuantity);

            if (quantity > product.StockQuantity)
                throw new InsufficientStockException(product.Id, product.Name.Value, quantity, product.StockQuantity);

            // بررسی اینکه آیا این محصول قبلاً در سفارش وجود دارد
            var existingItem = Order.OrderItems.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                // افزایش تعداد آیتم موجود
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                // ایجاد آیتم جدید
                var orderItem = new OrderItem(Order.Id, product.Id, quantity, unitPrice);
                Order.AddOrderItem(orderItem);
            }

            // کاهش موجودی محصول
            product.UpdateStock(product.StockQuantity - quantity);
            
            AddDomainEvent(new ProductStockUpdatedEvent(product.Id, product.Name.Value, product.SKU.Value, 
                product.StockQuantity + quantity, product.StockQuantity));
        }

        public void RemoveOrderItem(int productId)
        {
            var item = Order.OrderItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new ArgumentException($"Product {productId} not found in order");

            var quantity = item.Quantity;
            Order.RemoveOrderItem(item.Id);

            // بازگرداندن موجودی محصول
            // اینجا باید Product را از repository دریافت کنیم
            // برای حال حاضر فقط event را اضافه می‌کنیم
            AddDomainEvent(new ProductStockUpdatedEvent(productId, "", "", 0, quantity));
        }

        public void UpdateOrderStatus(OrderStatus newStatus)
        {
            if (Order.Status == newStatus)
                return;

            var oldStatus = Order.Status;
            
            // بررسی قوانین تغییر وضعیت
            if (!CanChangeStatus(oldStatus, newStatus))
                throw new InvalidOrderStatusException(Order.Id, oldStatus, newStatus);

            Order.UpdateStatus(newStatus);
            
            AddDomainEvent(new OrderStatusChangedEvent(Order.Id, Order.CustomerId, Order.OrderNumber.Value, 
                oldStatus, newStatus));
        }

        public void ApplyDiscount(Money discountAmount)
        {
            if (discountAmount.Amount > Order.TotalAmount.Amount * 0.3m)
                throw new DomainException("Discount cannot exceed 30% of total amount");

            Order.ApplyDiscount(discountAmount);
        }

        public void SetShippingCost(Money cost)
        {
            Order.SetShippingCost(cost);
        }

        public void SetTaxAmount(Money tax)
        {
            Order.SetTaxAmount(tax);
        }

        public void SetTrackingNumber(string trackingNumber)
        {
            Order.SetTrackingNumber(trackingNumber);
        }

        public void SetEstimatedDeliveryDate(DateTime estimatedDate)
        {
            Order.SetEstimatedDeliveryDate(estimatedDate);
        }

        public void UpdateNotes(string notes)
        {
            Order.UpdateNotes(notes);
        }

        public void UpdateShippingAddress(Address address)
        {
            Order.UpdateShippingAddress(address);
        }

        public void UpdateBillingAddress(Address address)
        {
            Order.UpdateBillingAddress(address);
        }

        public bool CanBeCancelled()
        {
            return Order.CanBeCancelled();
        }

        public bool CanBeRefunded()
        {
            return Order.Status == OrderStatus.Delivered && 
                   Order.DeliveredAt.HasValue &&
                   DateTime.UtcNow <= Order.DeliveredAt.Value.AddDays(7);
        }

        public void Cancel()
        {
            if (!CanBeCancelled())
                throw new InvalidOrderStatusException(Order.Id, Order.Status, "cancel");

            UpdateOrderStatus(OrderStatus.Cancelled);
            
            // بازگرداندن موجودی محصولات
            foreach (var item in Order.OrderItems)
            {
                AddDomainEvent(new ProductStockUpdatedEvent(item.ProductId, "", "", 0, item.Quantity));
            }
        }

        public void Refund()
        {
            if (!CanBeRefunded())
                throw new InvalidOrderStatusException(Order.Id, Order.Status, "refund");

            UpdateOrderStatus(OrderStatus.Refunded);
        }

        private bool CanChangeStatus(OrderStatus currentStatus, OrderStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (OrderStatus.Pending, OrderStatus.Confirmed) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Confirmed, OrderStatus.Processing) => true,
                (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Shipped, OrderStatus.Delivered) => true,
                (OrderStatus.Delivered, OrderStatus.Refunded) => true,
                (OrderStatus.Delivered, OrderStatus.Returned) => true,
                _ => false
            };
        }

        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
} 