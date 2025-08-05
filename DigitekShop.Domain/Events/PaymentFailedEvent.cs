using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class PaymentFailedEvent : BaseDomainEvent
    {
        public Payment Payment { get; }
        public string FailureReason { get; }

        public PaymentFailedEvent(Payment payment, string failureReason) : base("Payment", payment.Id.ToString())
        {
            Payment = payment;
            FailureReason = failureReason;
        }
    }
} 