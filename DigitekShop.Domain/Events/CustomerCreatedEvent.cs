using System;

namespace DigitekShop.Domain.Events
{
    public class CustomerCreatedEvent : BaseDomainEvent
    {
        public int CustomerId { get; }
        public string CustomerNumber { get; }
        public string CustomerName { get; }
        public string Email { get; }

        public CustomerCreatedEvent(int customerId, string customerNumber, string customerName = "", string email = "")
            : base("Customer", customerId.ToString())
        {
            CustomerId = customerId;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
            Email = email;
        }
    }
} 