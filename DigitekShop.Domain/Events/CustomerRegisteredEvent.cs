using System;

namespace DigitekShop.Domain.Events
{
    public class CustomerRegisteredEvent : IDomainEvent
    {
        public int CustomerId { get; }
        public string FullName { get; }
        public string Email { get; }
        public string Phone { get; }
        public DateTime OccurredOn { get; }

        public CustomerRegisteredEvent(int customerId, string fullName, string email, string phone)
        {
            CustomerId = customerId;
            FullName = fullName;
            Email = email;
            Phone = phone;
            OccurredOn = DateTime.UtcNow;
        }
    }
} 