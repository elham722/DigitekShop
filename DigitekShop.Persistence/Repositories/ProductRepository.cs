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
            return await _dbSet.Where(p => p.CategoryId == categoryId && !p.IsDeleted).ToListAsync(cancellationToken);
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
                .Where(p => !p.IsDeleted && p.Name.Value.Contains(searchTerm))
                .ToListAsync(cancellationToken);
        }
    }
}
