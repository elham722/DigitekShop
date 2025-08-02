using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DigitekShopDBContext _context;

        public OrderRepository(DigitekShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByOrderNumberAsync(OrderNumber orderNumber, CancellationToken cancellationToken = default)
        {
            // OrderNumber یک ValueObject است، پس باید مقایسه مناسب انجام شود
            return await _dbSet.AnyAsync(o => o.OrderNumber.Value == orderNumber.Value && !o.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(o => o.CustomerId == customerId && !o.IsDeleted)
                .Include(o => o.OrderItems) // اگر لازم است آیتم‌ها را هم بارگذاری کنیم
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order> GetByOrderNumberAsync(OrderNumber orderNumber, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderNumber.Value == orderNumber.Value && !o.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.Status == status && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(o => o.Status == status && !o.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetOverdueOrdersAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .Where(o => (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed)
                            && o.EstimatedDeliveryDate.HasValue
                            && o.EstimatedDeliveryDate < now
                            && !o.IsDeleted)
                .OrderBy(o => o.EstimatedDeliveryDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(o => o.Status == OrderStatus.Pending && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(o => o.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(o => o.CreatedAt <= endDate.Value);

            query = query.Where(o => !o.IsDeleted && o.Status == OrderStatus.Delivered);

            return await query.SumAsync(o => o.FinalAmount.Amount, cancellationToken);
        }

        public override async Task<Order> GetByIdWithIncludesAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }
    }
}
