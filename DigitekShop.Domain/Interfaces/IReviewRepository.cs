using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetByProductAsync(int productId);
        Task<IEnumerable<Review>> GetByCustomerAsync(int customerId);
        Task<IEnumerable<Review>> GetApprovedReviewsAsync(int productId);
        Task<IEnumerable<Review>> GetPendingReviewsAsync();
        Task<IEnumerable<Review>> GetByRatingAsync(int productId, int rating);
        Task<bool> ExistsByCustomerAndProductAsync(int customerId, int productId);
        Task<int> GetCountByProductAsync(int productId);
        Task<double> GetAverageRatingAsync(int productId);
        Task<int> GetCountByRatingAsync(int productId, int rating);
    }
} 