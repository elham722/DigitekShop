using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Interfaces
{
    public interface IWishlistRepository : IGenericRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Wishlist>> GetActiveByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
        Task<Wishlist> GetByCustomerAndProductAsync(int customerId, int productId, CancellationToken cancellationToken = default);
        Task<bool> ExistsByCustomerAndProductAsync(int customerId, int productId, CancellationToken cancellationToken = default);
        Task<int> GetCountByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
        Task<int> GetActiveCountByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    }
} 