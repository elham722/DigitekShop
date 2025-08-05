using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class PaymentCompletedEvent : BaseDomainEvent
    {
        public Payment Payment { get; }

        public PaymentCompletedEvent(Payment payment) : base("Payment", payment.Id.ToString())
        {
            Payment = payment;
        }
    }
} 