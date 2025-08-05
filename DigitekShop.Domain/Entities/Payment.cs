using System;
using System.Collections.Generic;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Events;
using DigitekShop.Domain.Enums;
using System.Linq; // Added for .Where() and .Sum()

namespace DigitekShop.Domain.Entities
{
    public class Payment : BaseAggregateRoot
    {
        // Core Properties
        public int OrderId { get; private set; }
        public Order Order { get; private set; }
        public Money Amount { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string TransactionId { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public string GatewayResponse { get; private set; }
        public string GatewayName { get; private set; }
        public string Currency { get; private set; }
        public decimal ExchangeRate { get; private set; }
        public Money OriginalAmount { get; private set; }
        public string FailureReason { get; private set; }
        public int RetryCount { get; private set; }
        public DateTime? ExpiresAt { get; private set; }

        // Navigation Properties
        public ICollection<PaymentTransaction> Transactions { get; private set; } = new List<PaymentTransaction>();

        // Constructor
        private Payment() { } // برای EF Core

        public static Payment Create(int orderId, Money amount, PaymentMethod method, string gatewayName = "Default")
        {
            if (amount.Amount <= 0)
                throw new ArgumentException("Payment amount must be positive");

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                Method = method,
                Status = PaymentStatus.Pending,
                TransactionId = GenerateTransactionId(),
                PaymentDate = DateTime.UtcNow,
                GatewayName = gatewayName ?? "Default",
                Currency = amount.Currency,
                ExchangeRate = 1.0m,
                OriginalAmount = amount,
                RetryCount = 0,
                ExpiresAt = DateTime.UtcNow.AddHours(24) // 24 hours expiry
            };

            payment.SetUpdated();
            payment.AddDomainEvent(new PaymentCreatedEvent(payment));
            
            return payment;
        }

        // Business Methods
        public void ProcessPayment(string gatewayResponse = "", string transactionId = null)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment can only be processed when in Pending status");

            Status = PaymentStatus.Processing;
            GatewayResponse = gatewayResponse ?? "";
            TransactionId = transactionId ?? TransactionId;
            PaymentDate = DateTime.UtcNow;
            SetUpdated();

            // Add transaction record
            var transaction = PaymentTransaction.Create(
                this.Id,
                PaymentTransactionType.Processing,
                Status.ToString(),
                gatewayResponse,
                "System"
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new PaymentProcessingEvent(this));
        }

        public void CompletePayment(string gatewayResponse = "", string transactionId = null)
        {
            if (Status != PaymentStatus.Processing && Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment can only be completed when in Processing or Pending status");

            Status = PaymentStatus.Completed;
            GatewayResponse = gatewayResponse ?? GatewayResponse;
            TransactionId = transactionId ?? TransactionId;
            PaymentDate = DateTime.UtcNow;
            SetUpdated();

            // Add transaction record
            var transaction = PaymentTransaction.Create(
                this.Id,
                PaymentTransactionType.Completed,
                Status.ToString(),
                gatewayResponse,
                "System"
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new PaymentCompletedEvent(this));
        }

        public void FailPayment(string failureReason, string gatewayResponse = "")
        {
            if (Status == PaymentStatus.Completed)
                throw new InvalidOperationException("Completed payment cannot be failed");

            Status = PaymentStatus.Failed;
            FailureReason = failureReason ?? "Unknown error";
            GatewayResponse = gatewayResponse ?? GatewayResponse;
            PaymentDate = DateTime.UtcNow;
            SetUpdated();

            // Add transaction record
            var transaction = PaymentTransaction.Create(
                this.Id,
                PaymentTransactionType.Failed,
                Status.ToString(),
                gatewayResponse,
                "System"
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new PaymentFailedEvent(this, failureReason));
        }

        public void CancelPayment(string reason = "")
        {
            if (Status == PaymentStatus.Completed)
                throw new InvalidOperationException("Completed payment cannot be cancelled");

            Status = PaymentStatus.Cancelled;
            FailureReason = reason ?? "Payment cancelled";
            PaymentDate = DateTime.UtcNow;
            SetUpdated();

            // Add transaction record
            var transaction = PaymentTransaction.Create(
                this.Id,
                PaymentTransactionType.Cancelled,
                Status.ToString(),
                reason,
                "System"
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new PaymentCancelledEvent(this, reason));
        }

        public void RefundPayment(Money refundAmount, string reason = "")
        {
            if (Status != PaymentStatus.Completed)
                throw new InvalidOperationException("Only completed payments can be refunded");

            if (refundAmount.Amount > Amount.Amount)
                throw new ArgumentException("Refund amount cannot exceed original payment amount");

            Status = PaymentStatus.Refunded;
            FailureReason = reason ?? "Payment refunded";
            PaymentDate = DateTime.UtcNow;
            SetUpdated();

            // Add transaction record
            var transaction = PaymentTransaction.Create(
                this.Id,
                PaymentTransactionType.Refunded,
                Status.ToString(),
                $"Refunded: {refundAmount.Amount} {refundAmount.Currency}. Reason: {reason}",
                "System"
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new PaymentRefundedEvent(this, refundAmount, reason));
        }

        public void RetryPayment()
        {
            if (Status != PaymentStatus.Failed)
                throw new InvalidOperationException("Only failed payments can be retried");

            if (RetryCount >= 3)
                throw new InvalidOperationException("Maximum retry count exceeded");

            Status = PaymentStatus.Pending;
            RetryCount++;
            PaymentDate = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddHours(24);
            SetUpdated();

            // Add transaction record
            var transaction = PaymentTransaction.Create(
                this.Id,
                PaymentTransactionType.Retry,
                Status.ToString(),
                $"Retry attempt {RetryCount}",
                "System"
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new PaymentRetryEvent(this, RetryCount));
        }

        public void UpdateGatewayResponse(string gatewayResponse)
        {
            GatewayResponse = gatewayResponse ?? "";
            SetUpdated();
        }

        public void UpdateTransactionId(string transactionId)
        {
            TransactionId = transactionId ?? TransactionId;
            SetUpdated();
        }

        // Query Methods
        public bool IsCompleted() => Status == PaymentStatus.Completed;
        public bool IsFailed() => Status == PaymentStatus.Failed;
        public bool IsPending() => Status == PaymentStatus.Pending;
        public bool IsProcessing() => Status == PaymentStatus.Processing;
        public bool IsCancelled() => Status == PaymentStatus.Cancelled;
        public bool IsRefunded() => Status == PaymentStatus.Refunded;
        public bool CanRetry() => Status == PaymentStatus.Failed && RetryCount < 3;
        public bool IsExpired() => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
        public bool CanRefund() => Status == PaymentStatus.Completed;

        public decimal GetRefundableAmount()
        {
            if (!IsCompleted()) return 0;
            
            var refundedTransactions = Transactions
                .Where(t => t.Type == PaymentTransactionType.Refunded)
                .Sum(t => ExtractRefundAmount(t.Description));
            
            return Math.Max(0, Amount.Amount - refundedTransactions);
        }

        // Private Methods
        private static string GenerateTransactionId()
        {
            return $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private static decimal ExtractRefundAmount(string description)
        {
            // Simple extraction - in real implementation, you might want more sophisticated parsing
            if (description.Contains("Refunded:"))
            {
                var parts = description.Split(' ');
                if (parts.Length > 1 && decimal.TryParse(parts[1], out var amount))
                    return amount;
            }
            return 0;
        }
    }
} 