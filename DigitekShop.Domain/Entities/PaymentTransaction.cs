using System;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Entities
{
    public class PaymentTransaction : BaseEntity
    {
        // Core Properties
        public int PaymentId { get; private set; }
        public Payment Payment { get; private set; }
        public PaymentTransactionType Type { get; private set; }
        public string Status { get; private set; }
        public string Description { get; private set; }
        public string GatewayResponse { get; private set; }
        public string ProcessedBy { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string ReferenceNumber { get; private set; }

        // Constructor
        private PaymentTransaction() { } // برای EF Core

        public static PaymentTransaction Create(int paymentId, PaymentTransactionType type, 
            string status, string description = "", string gatewayResponse = "", string processedBy = "System")
        {
            var transaction = new PaymentTransaction
            {
                PaymentId = paymentId,
                Type = type,
                Status = status ?? "",
                Description = description ?? "",
                GatewayResponse = gatewayResponse ?? "",
                ProcessedBy = processedBy ?? "System",
                TransactionDate = DateTime.UtcNow,
                ReferenceNumber = GenerateReferenceNumber()
            };

            transaction.SetUpdated();
            return transaction;
        }

        // Private Methods
        private static string GenerateReferenceNumber()
        {
            return $"PTX-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        // Query Methods
        public bool IsSuccessful() => Type == PaymentTransactionType.Completed;
        public bool IsFailed() => Type == PaymentTransactionType.Failed;
        public bool IsProcessing() => Type == PaymentTransactionType.Processing;
        public bool IsPending() => Type == PaymentTransactionType.Pending;
        public bool IsCancelled() => Type == PaymentTransactionType.Cancelled;
        public bool IsRefunded() => Type == PaymentTransactionType.Refunded;
        public bool IsRetry() => Type == PaymentTransactionType.Retry;
    }
} 