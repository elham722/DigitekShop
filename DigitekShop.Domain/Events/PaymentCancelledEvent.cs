using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class PaymentCancelledEvent : BaseDomainEvent
    {
        public Payment Payment { get; }
        public string CancellationReason { get; }

        public PaymentCancelledEvent(Payment payment, string cancellationReason) : base("Payment", payment.Id.ToString())
        {
            Payment = payment;
            CancellationReason = cancellationReason;
        }
    }
} 