using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Interfaces
{
    public interface IWishlistRepository : IGenericRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetByCustomerAsync(int customerId);
        Task<IEnumerable<Wishlist>> GetActiveByCustomerAsync(int customerId);
        Task<Wishlist> GetByCustomerAndProductAsync(int customerId, int productId);
        Task<bool> ExistsByCustomerAndProductAsync(int customerId, int productId);
        Task<int> GetCountByCustomerAsync(int customerId);
        Task<int> GetActiveCountByCustomerAsync(int customerId);
    }
} 