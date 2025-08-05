using DigitekShop.Application.DTOs.Inventory;
using DigitekShop.Application.Features.Inventory.Queries.GetInventory;
using DigitekShop.Application.Interfaces;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.Exceptions;
using AutoMapper;
using DigitekShop.Application.Exceptions;

namespace DigitekShop.Application.Features.Inventory.Queries.GetInventory
{
    public class GetInventoryQueryHandler : IQueryHandler<GetInventoryQuery, InventoryDto>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public GetInventoryQueryHandler(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<InventoryDto> HandleAsync(GetInventoryQuery query, CancellationToken cancellationToken)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(query.Id);
            if (inventory == null)
                throw new NotFoundException($"Inventory with ID {query.Id} not found");

            var inventoryDto = _mapper.Map<InventoryDto>(inventory);
            
            // Set additional properties
            inventoryDto.ProductName = inventory.Product?.Name.Value;
            inventoryDto.ProductSKU = inventory.Product?.SKU.Value;
            inventoryDto.CategoryName = inventory.Product?.Category?.Name.Value;
            inventoryDto.BrandName = inventory.Product?.Brand?.Name.Value;
            inventoryDto.StockUtilizationPercentage = inventory.GetStockUtilizationPercentage();
            inventoryDto.IsInStock = inventory.IsInStock();
            inventoryDto.IsLowStock = inventory.IsLowStock();
            inventoryDto.IsOutOfStock = inventory.IsOutOfStock();
            inventoryDto.IsOverstocked = inventory.IsOverstocked();

            return inventoryDto;
        }
    }
} 