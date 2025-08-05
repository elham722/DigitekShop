using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitekShop.Domain.Interfaces
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        // Basic CRUD
        Task<Inventory> GetByProductIdAsync(int productId);
        Task<IEnumerable<Inventory>> GetByStatusAsync(InventoryStatus status);
        Task<IEnumerable<Inventory>> GetLowStockItemsAsync();
        Task<IEnumerable<Inventory>> GetOutOfStockItemsAsync();
        Task<IEnumerable<Inventory>> GetOverstockedItemsAsync();

        // Stock Management
        Task<bool> ReserveStockAsync(int inventoryId, int quantity);
        Task<bool> ReleaseStockAsync(int inventoryId, int quantity);
        Task<bool> ConsumeStockAsync(int inventoryId, int quantity);
        Task<bool> UpdateStockAsync(int inventoryId, int newQuantity, string reason = "", string changedBy = "System");

        // Queries
        Task<int> GetTotalStockQuantityAsync();
        Task<int> GetTotalReservedQuantityAsync();
        Task<int> GetTotalAvailableQuantityAsync();
        Task<decimal> GetAverageStockUtilizationAsync();
        Task<IEnumerable<Inventory>> GetByLocationAsync(string location);
        Task<IEnumerable<Inventory>> GetByWarehouseCodeAsync(string warehouseCode);

        // Analytics
        Task<IEnumerable<Inventory>> GetTopSellingProductsAsync(int count = 10);
        Task<IEnumerable<Inventory>> GetSlowMovingProductsAsync(int daysThreshold = 30);
        Task<IEnumerable<Inventory>> GetProductsNeedingRestockAsync();
        Task<IEnumerable<Inventory>> GetProductsWithExpiringStockAsync(int daysThreshold = 7);

        // Transaction History
        Task<IEnumerable<InventoryTransaction>> GetTransactionHistoryAsync(int inventoryId, int page = 1, int pageSize = 20);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByTypeAsync(InventoryTransactionType type, int page = 1, int pageSize = 20);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByDateRangeAsync(DateTime fromDate, DateTime toDate);

        // Business Rules
        Task<bool> CanReserveStockAsync(int inventoryId, int quantity);
        Task<bool> IsLowStockAsync(int inventoryId);
        Task<bool> IsOutOfStockAsync(int inventoryId);
        Task<bool> IsOverstockedAsync(int inventoryId);
    }
} 