using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.Repositories
{
    public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
    {
        private readonly DigitekShopDBContext _context;

        public WishlistRepository(DigitekShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByCustomerAndProductAsync(int customerId, int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(w => w.CustomerId == customerId && w.ProductId == productId && !w.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Wishlist>> GetActiveByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(w => w.CustomerId == customerId && !w.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetActiveCountByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(w => w.CustomerId == customerId && !w.IsDeleted, cancellationToken);
        }

        public async Task<Wishlist> GetByCustomerAndProductAsync(int customerId, int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(w => w.CustomerId == customerId && w.ProductId == productId && !w.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Wishlist>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            // شامل موارد حذف نرم شده هم می‌شود، اگر می‌خواهی فقط آیتم‌های فعال، از GetActiveByCustomerAsync استفاده کن
            return await _dbSet
                .Where(w => w.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(w => w.CustomerId == customerId, cancellationToken);
        }
    }
}
