using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class PaymentRetryEvent : BaseDomainEvent
    {
        public Payment Payment { get; }
        public int RetryCount { get; }

        public PaymentRetryEvent(Payment payment, int retryCount) : base("Payment", payment.Id.ToString())
        {
            Payment = payment;
            RetryCount = retryCount;
        }
    }
} 