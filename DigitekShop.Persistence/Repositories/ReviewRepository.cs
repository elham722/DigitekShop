using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly DigitekShopDBContext _context;

        public ReviewRepository(DigitekShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByCustomerAndProductAsync(int customerId, int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(r => r.CustomerId == customerId && r.ProductId == productId && !r.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Review>> GetApprovedReviewsAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.ProductId == productId && r.IsApproved && !r.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<double> GetAverageRatingAsync(int productId, CancellationToken cancellationToken = default)
        {
            var ratings = await _dbSet
                .Where(r => r.ProductId == productId && r.IsApproved && !r.IsDeleted)
                .Select(r => r.Rating)
                .ToListAsync(cancellationToken);

            if (ratings.Count == 0)
                return 0;

            return ratings.Average();
        }

        public async Task<IEnumerable<Review>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.CustomerId == customerId && !r.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Review>> GetByProductAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.ProductId == productId && !r.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Review>> GetByRatingAsync(int productId, int rating, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.ProductId == productId && r.Rating == rating && !r.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountByProductAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .CountAsync(r => r.ProductId == productId && !r.IsDeleted, cancellationToken);
        }

        public async Task<int> GetCountByRatingAsync(int productId, int rating, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .CountAsync(r => r.ProductId == productId && r.Rating == rating && !r.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Review>> GetPendingReviewsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => !r.IsApproved && !r.IsDeleted)
                .ToListAsync(cancellationToken);
        }
    }
}
