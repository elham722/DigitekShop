using System;
using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class OrderCreatedEvent : BaseDomainEvent
    {
        public int OrderId { get; }
        public int CustomerId { get; }
        public string OrderNumber { get; }
        public decimal TotalAmount { get; }
        public int ItemCount { get; }
        public string PaymentMethod { get; }
        public string ShippingMethod { get; }

        public OrderCreatedEvent(Order order) 
            : base("Order", order.Id.ToString())
        {
            OrderId = order.Id;
            CustomerId = order.CustomerId;
            OrderNumber = order.OrderNumber.Value;
            TotalAmount = order.TotalAmount.Amount;
            ItemCount = order.GetItemCount();
            PaymentMethod = order.PaymentMethod.ToString();
            ShippingMethod = order.ShippingMethod;
        }
    }
} 