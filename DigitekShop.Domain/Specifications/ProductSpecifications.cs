using System;
using System.Linq.Expressions;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.Specifications
{
    public class ProductByCategorySpecification : BaseSpecification<Product>
    {
        public ProductByCategorySpecification(int categoryId)
        {
            AddCriteria(p => p.CategoryId == categoryId && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Name.Value);
        }
    }

    public class ProductByBrandSpecification : BaseSpecification<Product>
    {
        public ProductByBrandSpecification(int brandId)
        {
            AddCriteria(p => p.BrandId == brandId && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Name.Value);
        }
    }

    public class ProductInStockSpecification : BaseSpecification<Product>
    {
        public ProductInStockSpecification()
        {
            AddCriteria(p => p.StockQuantity > 0 && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Name.Value);
        }
    }

    public class ProductOutOfStockSpecification : BaseSpecification<Product>
    {
        public ProductOutOfStockSpecification()
        {
            AddCriteria(p => p.StockQuantity == 0 && p.Status == ProductStatus.OutOfStock);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Name.Value);
        }
    }

    public class ProductLowStockSpecification : BaseSpecification<Product>
    {
        public ProductLowStockSpecification(int threshold = 10)
        {
            AddCriteria(p => p.StockQuantity <= threshold && p.StockQuantity > 0 && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.StockQuantity);
        }
    }

    public class ProductByPriceRangeSpecification : BaseSpecification<Product>
    {
        public ProductByPriceRangeSpecification(decimal minPrice, decimal maxPrice)
        {
            AddCriteria(p => p.Price.Amount >= minPrice && p.Price.Amount <= maxPrice && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Price.Amount);
        }
    }

    public class ProductExpensiveSpecification : BaseSpecification<Product>
    {
        public ProductExpensiveSpecification(decimal threshold = 1000000m)
        {
            AddCriteria(p => p.Price.Amount >= threshold && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderByDescending(p => p.Price.Amount);
        }
    }

    public class ProductCheapSpecification : BaseSpecification<Product>
    {
        public ProductCheapSpecification(decimal threshold = 500000m)
        {
            AddCriteria(p => p.Price.Amount <= threshold && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Price.Amount);
        }
    }

    public class ProductByStatusSpecification : BaseSpecification<Product>
    {
        public ProductByStatusSpecification(ProductStatus status)
        {
            AddCriteria(p => p.Status == status);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Name.Value);
        }
    }

    public class ProductSearchSpecification : BaseSpecification<Product>
    {
        public ProductSearchSpecification(string searchTerm)
        {
            var term = searchTerm.ToLower();
            AddCriteria(p => (p.Name.Value.ToLower().Contains(term) || 
                             p.Description.ToLower().Contains(term) || 
                             p.Model.ToLower().Contains(term)) && 
                             p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Name.Value);
        }
    }

    public class ProductWithReviewsSpecification : BaseSpecification<Product>
    {
        public ProductWithReviewsSpecification()
        {
            AddCriteria(p => p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddInclude(p => p.Reviews);
            AddOrderByDescending(p => p.Reviews.Count);
        }
    }

    public class ProductByRatingSpecification : BaseSpecification<Product>
    {
        public ProductByRatingSpecification(decimal minRating)
        {
            var minRatingObj = new Rating(minRating);
            AddCriteria(p => p.Status == ProductStatus.Active && 
                             p.Reviews.Any(r => r.IsApproved) &&
                             p.GetAverageRating() >= minRatingObj);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddInclude(p => p.Reviews);
            AddOrderByDescending(p => p.GetAverageRating().Value);
        }
    }

    public class ProductNewArrivalsSpecification : BaseSpecification<Product>
    {
        public ProductNewArrivalsSpecification(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            AddCriteria(p => p.CreatedAt >= cutoffDate && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderByDescending(p => p.CreatedAt);
        }
    }

    public class ProductBestSellersSpecification : BaseSpecification<Product>
    {
        public ProductBestSellersSpecification()
        {
            AddCriteria(p => p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            // Note: This would need to be implemented with actual sales data
            AddOrderByDescending(p => p.Id); // Placeholder
        }
    }

    public class ProductByWeightSpecification : BaseSpecification<Product>
    {
        public ProductByWeightSpecification(decimal minWeight, decimal maxWeight)
        {
            AddCriteria(p => p.Weight >= minWeight && p.Weight <= maxWeight && p.Status == ProductStatus.Active);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Weight);
        }
    }

    public class ProductWithPagingSpecification : BaseSpecification<Product>
    {
        public ProductWithPagingSpecification(int page, int pageSize, string searchTerm = null, int? categoryId = null)
        {
            var skip = (page - 1) * pageSize;
            ApplyPaging(skip, pageSize);

            Expression<Func<Product, bool>> criteria = p => p.Status == ProductStatus.Active;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                criteria = p => p.Status == ProductStatus.Active && 
                               (p.Name.Value.ToLower().Contains(term) || 
                                p.Description.ToLower().Contains(term) || 
                                p.Model.ToLower().Contains(term));
            }

            if (categoryId.HasValue)
            {
                criteria = p => p.Status == ProductStatus.Active && p.CategoryId == categoryId.Value;
            }

            AddCriteria(criteria);
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddOrderBy(p => p.Name.Value);
        }
    }
} 