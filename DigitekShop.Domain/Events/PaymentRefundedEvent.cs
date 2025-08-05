using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.Events
{
    public class PaymentRefundedEvent : BaseDomainEvent
    {
        public Payment Payment { get; }
        public Money RefundAmount { get; }
        public string RefundReason { get; }

        public PaymentRefundedEvent(Payment payment, Money refundAmount, string refundReason) : base("Payment", payment.Id.ToString())
        {
            Payment = payment;
            RefundAmount = refundAmount;
            RefundReason = refundReason;
        }
    }
} 