using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class PaymentProcessingEvent : BaseDomainEvent
    {
        public Payment Payment { get; }

        public PaymentProcessingEvent(Payment payment) : base("Payment", payment.Id.ToString())
        {
            Payment = payment;
        }
    }
} 