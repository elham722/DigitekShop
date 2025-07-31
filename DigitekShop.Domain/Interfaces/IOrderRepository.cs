using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetByOrderNumberAsync(OrderNumber orderNumber);
        Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
        Task<IEnumerable<Order>> GetOverdueOrdersAsync();
        Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> ExistsByOrderNumberAsync(OrderNumber orderNumber);
        Task<int> GetCountByStatusAsync(OrderStatus status);
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
} 