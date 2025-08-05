namespace DigitekShop.Application.DTOs.Inventory
{
    public class UpdateInventoryDto
    {
        public int? NewQuantity { get; set; }
        public int? MinimumStockLevel { get; set; }
        public int? MaximumStockLevel { get; set; }
        public string? Location { get; set; }
        public string? WarehouseCode { get; set; }
        public string? Reason { get; set; }
        public string? ChangedBy { get; set; }
    }
} 