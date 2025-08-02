using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitekShop.Tests.Mocks
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        private int _nextId = 1;

        public MockProductRepository()
        {
            _products = new List<Product>();
            SeedMockData();
        }

        public MockProductRepository(List<Product> products)
        {
            _products = products ?? new List<Product>();
            _nextId = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
        }

        private void SeedMockData()
        {
            // Add some sample products for testing
            var category = new Category(new ProductName("الکترونیک"), "محصولات الکترونیکی", CategoryType.Main);
            var brand = new Brand(new ProductName("اپل"), "شرکت اپل آمریکا");

            var products = new[]
            {
                new Product(
                    new ProductName("iPhone 15"),
                    "گوشی هوشمند اپل آیفون 15",
                    new Money(85000000, "IRR"),
                    10,
                    SKU.Generate("PHONE", "APPLE", "IP15"),
                    1,
                    1,
                    "iPhone 15",
                    0.2m
                ),
                new Product(
                    new ProductName("MacBook Air"),
                    "لپ تاپ اپل مک بوک ایر",
                    new Money(75000000, "IRR"),
                    5,
                    SKU.Generate("LAPTOP", "APPLE", "MBA"),
                    1,
                    1,
                    "MacBook Air",
                    1.3m
                ),
                new Product(
                    new ProductName("AirPods Pro"),
                    "هدفون بی‌سیم اپل ایرپادز پرو",
                    new Money(8500000, "IRR"),
                    20,
                    SKU.Generate("ACCESSORY", "APPLE", "APP"),
                    1,
                    1,
                    "AirPods Pro",
                    0.045m
                )
            };

            foreach (var product in products)
            {
                product.GetType().GetProperty("Id")?.SetValue(product, _nextId++);
                _products.Add(product);
            }
        }

        // IProductRepository specific methods
        public async Task<Product> GetBySKUAsync(SKU sku, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken); // Simulate async operation
            return _products.FirstOrDefault(p => p.SKU.Value == sku.Value);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Where(p => p.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Where(p => p.Status == status);
        }

        public async Task<IEnumerable<Product>> GetInStockAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Where(p => p.IsInStock());
        }

        public async Task<IEnumerable<Product>> GetExpensiveProductsAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Where(p => p.IsExpensive());
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Where(p => p.IsLowStock(threshold));
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Product>();

            return _products.Where(p => 
                p.Name.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> ExistsBySKUAsync(SKU sku, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Any(p => p.SKU.Value == sku.Value);
        }

        public async Task<int> GetCountByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Count(p => p.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Product>> GetProductsWithFiltersAsync(
            int? categoryId = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            ProductStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            
            var query = _products.AsQueryable();

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

            return query.ToList();
        }

        public async Task<decimal> GetAverageRatingAsync(int productId, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var product = _products.FirstOrDefault(p => p.Id == productId);
            return product?.GetAverageRating() ?? 0;
        }

        public async Task<int> GetReviewCountAsync(int productId, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var product = _products.FirstOrDefault(p => p.Id == productId);
            return product?.GetReviewCount() ?? 0;
        }

        public async Task<IEnumerable<Product>> GetTopRatedProductsAsync(int count = 10, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products
                .OrderByDescending(p => p.GetAverageRating())
                .Take(count);
        }

        // IGenericRepository methods
        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Product> GetByIdWithIncludesAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.ToList();
        }

        public async Task<IEnumerable<Product>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Where(p => p.Status == ProductStatus.Active);
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var totalCount = _products.Count;
            var items = _products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetActivePagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var activeProducts = _products.Where(p => p.Status == ProductStatus.Active).ToList();
            var totalCount = activeProducts.Count;
            var items = activeProducts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (items, totalCount);
        }

        public async Task<Product> AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            entity.GetType().GetProperty("Id")?.SetValue(entity, _nextId++);
            _products.Add(entity);
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var productList = entities.ToList();
            foreach (var entity in productList)
            {
                entity.GetType().GetProperty("Id")?.SetValue(entity, _nextId++);
                _products.Add(entity);
            }
            return productList;
        }

        public async Task<Product> UpdateAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var existingProduct = _products.FirstOrDefault(p => p.Id == entity.Id);
            if (existingProduct != null)
            {
                var index = _products.IndexOf(existingProduct);
                _products[index] = entity;
            }
            return entity;
        }

        public async Task UpdateRangeAsync(IEnumerable<Product> entities, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, cancellationToken);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }

        public async Task DeleteAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            _products.Remove(entity);
        }

        public async Task DeleteRangeAsync(IEnumerable<Product> entities, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            foreach (var entity in entities)
            {
                _products.Remove(entity);
            }
        }

        public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.Deactivate();
            }
        }

        public async Task SoftDeleteAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            entity.Deactivate();
        }

        public async Task RestoreAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.Activate();
            }
        }

        public async Task RestoreAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            entity.Activate();
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Any(p => p.Id == id);
        }

        public async Task<bool> ExistsAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Any(p => p.Id == entity.Id);
        }

        public async Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _products.Any(p => p.Id == id && p.Status == ProductStatus.Active);
        }

        public async Task<int> GetTotalCountAsync()
        {
            await Task.Delay(1);
            return _products.Count;
        }

        public async Task<int> GetActiveCountAsync()
        {
            await Task.Delay(1);
            return _products.Count(p => p.Status == ProductStatus.Active);
        }

        // Helper methods for testing
        public void Clear()
        {
            _products.Clear();
            _nextId = 1;
        }

        public void AddProduct(Product product)
        {
            product.GetType().GetProperty("Id")?.SetValue(product, _nextId++);
            _products.Add(product);
        }

        public List<Product> GetAllProducts()
        {
            return _products.ToList();
        }
    }
} 