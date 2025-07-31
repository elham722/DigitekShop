using System;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Events
{
    public class OrderStatusChangedEvent : IDomainEvent
    {
        public int OrderId { get; }
        public int CustomerId { get; }
        public string OrderNumber { get; }
        public OrderStatus OldStatus { get; }
        public OrderStatus NewStatus { get; }
        public DateTime OccurredOn { get; }

        public OrderStatusChangedEvent(int orderId, int customerId, string orderNumber, 
            OrderStatus oldStatus, OrderStatus newStatus)
        {
            OrderId = orderId;
            CustomerId = customerId;
            OrderNumber = orderNumber;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            OccurredOn = DateTime.UtcNow;
        }
    }
} 