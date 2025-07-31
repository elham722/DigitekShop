using System;
using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class OrderCreatedEvent : IDomainEvent
    {
        public int OrderId { get; }
        public int CustomerId { get; }
        public string OrderNumber { get; }
        public decimal TotalAmount { get; }
        public int ItemCount { get; }
        public DateTime OccurredOn { get; }

        public OrderCreatedEvent(Order order)
        {
            OrderId = order.Id;
            CustomerId = order.CustomerId;
            OrderNumber = order.OrderNumber.Value;
            TotalAmount = order.TotalAmount.Amount;
            ItemCount = order.GetItemCount();
            OccurredOn = DateTime.UtcNow;
        }
    }
} 