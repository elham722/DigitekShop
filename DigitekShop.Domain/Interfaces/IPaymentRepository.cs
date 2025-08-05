using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitekShop.Domain.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        // Basic CRUD
        Task<Payment> GetByOrderIdAsync(int orderId);
        Task<Payment> GetByTransactionIdAsync(string transactionId);
        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
        Task<IEnumerable<Payment>> GetByPaymentMethodAsync(PaymentMethod method);

        // Payment Management
        Task<bool> ProcessPaymentAsync(int paymentId, string gatewayResponse = "", string transactionId = null);
        Task<bool> CompletePaymentAsync(int paymentId, string gatewayResponse = "", string transactionId = null);
        Task<bool> FailPaymentAsync(int paymentId, string failureReason, string gatewayResponse = "");
        Task<bool> CancelPaymentAsync(int paymentId, string reason = "");
        Task<bool> RefundPaymentAsync(int paymentId, decimal refundAmount, string reason = "");
        Task<bool> RetryPaymentAsync(int paymentId);

        // Queries
        Task<decimal> GetTotalPaymentsAsync();
        Task<decimal> GetTotalRefundsAsync();
        Task<decimal> GetTotalFailedPaymentsAsync();
        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Payment>> GetByGatewayAsync(string gatewayName);
        Task<IEnumerable<Payment>> GetExpiredPaymentsAsync();

        // Analytics
        Task<IEnumerable<Payment>> GetTopPaymentsAsync(int count = 10);
        Task<IEnumerable<Payment>> GetFailedPaymentsAsync(int daysThreshold = 7);
        Task<IEnumerable<Payment>> GetPendingPaymentsAsync();
        Task<IEnumerable<Payment>> GetPaymentsNeedingRetryAsync();

        // Transaction History
        Task<IEnumerable<PaymentTransaction>> GetTransactionHistoryAsync(int paymentId, int page = 1, int pageSize = 20);
        Task<IEnumerable<PaymentTransaction>> GetTransactionsByTypeAsync(PaymentTransactionType type, int page = 1, int pageSize = 20);
        Task<IEnumerable<PaymentTransaction>> GetTransactionsByDateRangeAsync(DateTime fromDate, DateTime toDate);

        // Business Rules
        Task<bool> CanRetryPaymentAsync(int paymentId);
        Task<bool> CanRefundPaymentAsync(int paymentId);
        Task<bool> IsPaymentExpiredAsync(int paymentId);
        Task<bool> HasPaymentForOrderAsync(int orderId);
    }
} 