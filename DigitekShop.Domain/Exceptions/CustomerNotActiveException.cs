using System;

namespace DigitekShop.Domain.Exceptions
{
    public class CustomerNotActiveException : DomainException
    {
        public int CustomerId { get; }

        public CustomerNotActiveException(int customerId) 
            : base($"Customer with ID {customerId} is not active and cannot perform this operation.")
        {
            CustomerId = customerId;
        }

        public CustomerNotActiveException(int customerId, string operation) 
            : base($"Customer with ID {customerId} is not active and cannot perform {operation}.")
        {
            CustomerId = customerId;
        }
    }
} 