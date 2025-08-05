using DigitekShop.Application.DTOs.Inventory;
using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Features.Inventory.Queries.GetInventory
{
    public record GetInventoryQuery : IQuery<InventoryDto>
    {
        public int Id { get; init; }
    }
} 