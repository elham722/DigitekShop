using System;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Exceptions
{
    public class InvalidOrderStatusException : DomainException
    {
        public int OrderId { get; }
        public OrderStatus CurrentStatus { get; }
        public OrderStatus RequestedStatus { get; }

        public InvalidOrderStatusException(int orderId, OrderStatus currentStatus, OrderStatus requestedStatus)
            : base($"Cannot change order {orderId} status from {currentStatus} to {requestedStatus}")
        {
            OrderId = orderId;
            CurrentStatus = currentStatus;
            RequestedStatus = requestedStatus;
        }

        public InvalidOrderStatusException(int orderId, OrderStatus currentStatus, string operation)
            : base($"Cannot perform {operation} on order {orderId} with status {currentStatus}")
        {
            OrderId = orderId;
            CurrentStatus = currentStatus;
        }
    }
} 