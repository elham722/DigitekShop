using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetBySKUAsync(SKU sku);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus status);
        Task<IEnumerable<Product>> GetInStockAsync();
        Task<IEnumerable<Product>> GetExpensiveProductsAsync();
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10);
        Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm);
        Task<bool> ExistsBySKUAsync(SKU sku);
        Task<int> GetCountByCategoryAsync(int categoryId);
    }
} 