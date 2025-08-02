using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetBySKUAsync(SKU sku, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken=default);
        Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken=default);
        Task<IEnumerable<Product>> GetInStockAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetExpensiveProductsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10, CancellationToken cancellationToken=default);
        Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySKUAsync(SKU sku, CancellationToken cancellationToken = default);
        Task<int> GetCountByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetProductsWithFiltersAsync(
            int? categoryId = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            ProductStatus? status = null,
            CancellationToken cancellationToken = default);
        Task<decimal> GetAverageRatingAsync(int productId, CancellationToken cancellationToken = default);
        Task<int> GetReviewCountAsync(int productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetTopRatedProductsAsync(int count = 10, CancellationToken cancellationToken = default);
    }
} 