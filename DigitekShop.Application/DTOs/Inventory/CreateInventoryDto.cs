namespace DigitekShop.Application.DTOs.Inventory
{
    public class CreateInventoryDto
    {
        public int ProductId { get; set; }
        public int InitialQuantity { get; set; }
        public int MinimumStockLevel { get; set; } = 10;
        public int MaximumStockLevel { get; set; } = 1000;
        public string Location { get; set; } = "Main Warehouse";
        public string WarehouseCode { get; set; } = "MAIN";
    }
} 