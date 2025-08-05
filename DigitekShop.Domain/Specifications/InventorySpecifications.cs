using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Specifications
{
    public class InventorySpecifications
    {
        public class InventoryByStatusSpecification : BaseSpecification<Inventory>
        {
            public InventoryByStatusSpecification(InventoryStatus status)
            {
                AddCriteria(i => i.Status == status);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddInclude(i => i.Product.Brand);
            }
        }

        public class LowStockInventorySpecification : BaseSpecification<Inventory>
        {
            public LowStockInventorySpecification()
            {
                AddCriteria(i => i.IsLowStock() && i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderBy(i => i.AvailableQuantity);
            }
        }

        public class OutOfStockInventorySpecification : BaseSpecification<Inventory>
        {
            public OutOfStockInventorySpecification()
            {
                AddCriteria(i => i.IsOutOfStock() && i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderBy(i => i.LastUpdated);
            }
        }

        public class OverstockedInventorySpecification : BaseSpecification<Inventory>
        {
            public OverstockedInventorySpecification()
            {
                AddCriteria(i => i.IsOverstocked() && i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderByDescending(i => i.Quantity);
            }
        }

        public class InventoryByLocationSpecification : BaseSpecification<Inventory>
        {
            public InventoryByLocationSpecification(string location)
            {
                AddCriteria(i => i.Location == location && i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderBy(i => i.Product.Name.Value);
            }
        }

        public class InventoryByWarehouseSpecification : BaseSpecification<Inventory>
        {
            public InventoryByWarehouseSpecification(string warehouseCode)
            {
                AddCriteria(i => i.WarehouseCode == warehouseCode && i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderBy(i => i.Product.Name.Value);
            }
        }

        public class InventoryByProductIdSpecification : BaseSpecification<Inventory>
        {
            public InventoryByProductIdSpecification(int productId)
            {
                AddCriteria(i => i.ProductId == productId);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddInclude(i => i.Product.Brand);
            }
        }

        public class ActiveInventorySpecification : BaseSpecification<Inventory>
        {
            public ActiveInventorySpecification()
            {
                AddCriteria(i => i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderBy(i => i.Product.Name.Value);
            }
        }

        public class InventoryWithTransactionsSpecification : BaseSpecification<Inventory>
        {
            public InventoryWithTransactionsSpecification(int inventoryId)
            {
                AddCriteria(i => i.Id == inventoryId);
                AddInclude(i => i.Product);
                AddInclude(i => i.Transactions);
            }
        }

        public class InventoryByStockLevelSpecification : BaseSpecification<Inventory>
        {
            public InventoryByStockLevelSpecification(int minQuantity, int maxQuantity)
            {
                AddCriteria(i => i.Quantity >= minQuantity && i.Quantity <= maxQuantity && i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderBy(i => i.Quantity);
            }
        }

        public class InventoryByUtilizationSpecification : BaseSpecification<Inventory>
        {
            public InventoryByUtilizationSpecification(decimal minUtilization, decimal maxUtilization)
            {
                AddCriteria(i => i.GetStockUtilizationPercentage() >= minUtilization && 
                                i.GetStockUtilizationPercentage() <= maxUtilization && 
                                i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderByDescending(i => i.GetStockUtilizationPercentage());
            }
        }

        public class InventoryNeedingRestockSpecification : BaseSpecification<Inventory>
        {
            public InventoryNeedingRestockSpecification()
            {
                AddCriteria(i => i.IsLowStock() && i.Status == InventoryStatus.Active);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderBy(i => i.AvailableQuantity);
            }
        }

        public class InventoryByLastUpdatedSpecification : BaseSpecification<Inventory>
        {
            public InventoryByLastUpdatedSpecification(DateTime fromDate, DateTime toDate)
            {
                AddCriteria(i => i.LastUpdated >= fromDate && i.LastUpdated <= toDate);
                AddInclude(i => i.Product);
                AddInclude(i => i.Product.Category);
                AddOrderByDescending(i => i.LastUpdated);
            }
        }
    }
} 