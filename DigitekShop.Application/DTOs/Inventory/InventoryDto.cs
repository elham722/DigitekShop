using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Inventory
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public int MaximumStockLevel { get; set; }
        public string Location { get; set; }
        public string WarehouseCode { get; set; }
        public DateTime LastUpdated { get; set; }
        public InventoryStatus Status { get; set; }
        public decimal StockUtilizationPercentage { get; set; }
        public bool IsInStock { get; set; }
        public bool IsLowStock { get; set; }
        public bool IsOutOfStock { get; set; }
        public bool IsOverstocked { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
    }
} 