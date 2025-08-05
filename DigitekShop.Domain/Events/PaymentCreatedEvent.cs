using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class PaymentCreatedEvent : BaseDomainEvent
    {
        public Payment Payment { get; }

        public PaymentCreatedEvent(Payment payment) : base("Payment", payment.Id.ToString())
        {
            Payment = payment;
        }
    }
} 