using System;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Events
{
    public class OrderStatusChangedEvent : BaseDomainEvent
    {
        public int OrderId { get; }
        public int CustomerId { get; }
        public string OrderNumber { get; }
        public OrderStatus OldStatus { get; }
        public OrderStatus NewStatus { get; }
        public string ChangedBy { get; }
        public string Reason { get; }

        public OrderStatusChangedEvent(Order order, OrderStatus oldStatus, OrderStatus newStatus, string changedBy = "System", string reason = "")
            : base("Order", order.Id.ToString())
        {
            OrderId = order.Id;
            CustomerId = order.CustomerId;
            OrderNumber = order.OrderNumber.Value;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            ChangedBy = changedBy;
            Reason = reason;
        }
    }
} 