using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetByProductAsync(int productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Review>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Review>> GetApprovedReviewsAsync(int productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Review>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Review>> GetByRatingAsync(int productId, int rating, CancellationToken cancellationToken = default);
        Task<bool> ExistsByCustomerAndProductAsync(int customerId, int productId, CancellationToken cancellationToken = default);
        Task<int> GetCountByProductAsync(int productId, CancellationToken cancellationToken = default);
        Task<double> GetAverageRatingAsync(int productId, CancellationToken cancellationToken = default);
        Task<int> GetCountByRatingAsync(int productId, int rating, CancellationToken cancellationToken = default);
    }
} 