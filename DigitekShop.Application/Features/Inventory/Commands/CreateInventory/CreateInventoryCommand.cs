using DigitekShop.Application.DTOs.Inventory;
using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Features.Inventory.Commands.CreateInventory
{
    public record CreateInventoryCommand : ICommand<InventoryDto>
    {
        public int ProductId { get; init; }
        public int InitialQuantity { get; init; }
        public int MinimumStockLevel { get; init; } = 10;
        public int MaximumStockLevel { get; init; } = 1000;
        public string Location { get; init; } = "Main Warehouse";
        public string WarehouseCode { get; init; } = "MAIN";
    }
} 