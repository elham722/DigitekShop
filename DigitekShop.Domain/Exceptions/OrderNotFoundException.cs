using System;

namespace DigitekShop.Domain.Exceptions
{
    public class OrderNotFoundException : DomainException
    {
        public int OrderId { get; }

        public OrderNotFoundException(int orderId)
            : base($"Order with ID {orderId} was not found")
        {
            OrderId = orderId;
        }

        public OrderNotFoundException(string orderNumber)
            : base($"Order with number {orderNumber} was not found")
        {
        }
    }
} 