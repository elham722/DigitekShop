using System;
using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class CustomerRegisteredEvent : BaseDomainEvent
    {
        public int CustomerId { get; }
        public string FullName { get; }
        public string Email { get; }
        public string Phone { get; }
        public string NationalCode { get; }
        public DateTime? DateOfBirth { get; }

        public CustomerRegisteredEvent(Customer customer)
            : base("Customer", customer.Id.ToString())
        {
            CustomerId = customer.Id;
            FullName = customer.GetFullName();
            Email = customer.Email.Value;
            Phone = customer.Phone.Value;
            NationalCode = customer.NationalCode;
            DateOfBirth = customer.DateOfBirth;
        }
    }
} 