using System;

namespace DigitekShop.Domain.Exceptions
{
    public class CustomerNotFoundException : DomainException
    {
        public int CustomerId { get; }

        public CustomerNotFoundException(int customerId)
            : base($"Customer with ID {customerId} was not found")
        {
            CustomerId = customerId;
        }

        public CustomerNotFoundException(string email)
            : base($"Customer with email {email} was not found")
        {
        }
    }
} 