using System;
using System.Linq.Expressions;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Specifications
{
    public class ProductSpecifications
    {
        public class ActiveProductsSpecification : BaseSpecification<Product>
        {
            public ActiveProductsSpecification()
            {
                AddCriteria(p => p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.Name.Value);
            }
        }

        public class ProductsByCategorySpecification : BaseSpecification<Product>
        {
            public ProductsByCategorySpecification(int categoryId)
            {
                AddCriteria(p => p.CategoryId == categoryId && p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.Name.Value);
            }
        }

        public class ProductsByBrandSpecification : BaseSpecification<Product>
        {
            public ProductsByBrandSpecification(int brandId)
            {
                AddCriteria(p => p.BrandId == brandId && p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.Name.Value);
            }
        }

        public class InStockProductsSpecification : BaseSpecification<Product>
        {
            public InStockProductsSpecification()
            {
                AddCriteria(p => p.StockQuantity > 0 && p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.Name.Value);
            }
        }

        public class LowStockProductsSpecification : BaseSpecification<Product>
        {
            public LowStockProductsSpecification(int threshold = 10)
            {
                AddCriteria(p => p.StockQuantity <= threshold && p.StockQuantity > 0 && p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.StockQuantity);
            }
        }

        public class ExpensiveProductsSpecification : BaseSpecification<Product>
        {
            public ExpensiveProductsSpecification(decimal minPrice = 1000000m)
            {
                AddCriteria(p => p.Price.Amount >= minPrice && p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderByDescending(p => p.Price.Amount);
            }
        }

        public class AffordableProductsSpecification : BaseSpecification<Product>
        {
            public AffordableProductsSpecification(decimal maxPrice)
            {
                AddCriteria(p => p.Price.Amount <= maxPrice && p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.Price.Amount);
            }
        }

        public class ProductsByNameSpecification : BaseSpecification<Product>
        {
            public ProductsByNameSpecification(string searchTerm)
            {
                AddCriteria(p => p.Name.Value.Contains(searchTerm) && p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.Name.Value);
            }
        }

        public class ComingSoonProductsSpecification : BaseSpecification<Product>
        {
            public ComingSoonProductsSpecification()
            {
                AddCriteria(p => p.Status == ProductStatus.ComingSoon);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.CreatedAt);
            }
        }

        public class DiscontinuedProductsSpecification : BaseSpecification<Product>
        {
            public DiscontinuedProductsSpecification()
            {
                AddCriteria(p => p.Status == ProductStatus.Discontinued);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderByDescending(p => p.UpdatedAt);
            }
        }

        public class ProductsWithPagingSpecification : BaseSpecification<Product>
        {
            public ProductsWithPagingSpecification(int page, int pageSize)
            {
                AddCriteria(p => p.Status == ProductStatus.Active);
                AddInclude(p => p.Category);
                AddInclude(p => p.Brand);
                AddOrderBy(p => p.Name.Value);
                ApplyPaging((page - 1) * pageSize, pageSize);
            }
        }
    }
} 