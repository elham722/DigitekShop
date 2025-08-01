using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetByOrderNumberAsync(OrderNumber orderNumber, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetPendingOrdersAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetOverdueOrdersAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<bool> ExistsByOrderNumberAsync(OrderNumber orderNumber, CancellationToken cancellationToken = default);
        Task<int> GetCountByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    }
} 