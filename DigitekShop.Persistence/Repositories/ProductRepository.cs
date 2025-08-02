using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DigitekShopDBContext _context;

        public ProductRepository(DigitekShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsBySKUAsync(SKU sku, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(p => p.SKU == sku && !p.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> GetBySKUAsync(SKU sku, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.SKU == sku && !p.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(p => p.Status == status && !p.IsDeleted).ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(p => p.CategoryId == categoryId && !p.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetExpensiveProductsAsync(CancellationToken cancellationToken = default)
        {
            // مثلا تعریف یک حد قیمت بالا، مثلا بیشتر از 1000 (یا هر عددی که مد نظرت است)
            decimal expensiveThreshold = 1000m;
            return await _dbSet.Where(p => p.Price.Amount >= expensiveThreshold && !p.IsDeleted).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetInStockAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(p => p.StockQuantity > 0 && p.Status == ProductStatus.Active && !p.IsDeleted).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(p => p.StockQuantity <= threshold && p.StockQuantity > 0 && !p.IsDeleted).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            // چون نام محصول از نوع Value Object ProductName هست، باید مقدار Value را مقایسه کنیم
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => !p.IsDeleted && p.Name.Value.Contains(searchTerm))
                .ToListAsync(cancellationToken);
        }

        public override async Task<Product> GetByIdWithIncludesAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsWithFiltersAsync(
            int? categoryId = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            ProductStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => !p.IsDeleted);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (brandId.HasValue)
                query = query.Where(p => p.BrandId == brandId.Value);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price.Amount >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price.Amount <= maxPrice.Value);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetAverageRatingAsync(int productId, CancellationToken cancellationToken = default)
        {
            return (decimal)await _dbSet
                .Where(p => p.Id == productId && !p.IsDeleted)
                .SelectMany(p => p.Reviews)
                .Where(r => r.IsApproved)
                .AverageAsync(r => r.Rating, cancellationToken);
        }

        public async Task<int> GetReviewCountAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(p => p.Id == productId && !p.IsDeleted)
                .SelectMany(p => p.Reviews)
                .CountAsync(r => r.IsApproved, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetTopRatedProductsAsync(int count = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => !p.IsDeleted && p.Status == ProductStatus.Active)
                .Select(p => new
                {
                    Product = p,
                    AverageRating = p.Reviews.Where(r => r.IsApproved).Average(r => r.Rating)
                })
                .Where(x => x.AverageRating > 0)
                .OrderByDescending(x => x.AverageRating)
                .Take(count)
                .Select(x => x.Product)
                .ToListAsync(cancellationToken);
        }
    }
}
