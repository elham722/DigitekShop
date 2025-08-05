using DigitekShop.Application.DTOs.Inventory;
using DigitekShop.Application.Features.Inventory.Commands.CreateInventory;
using DigitekShop.Application.Interfaces;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.Exceptions;
using DigitekShop.Domain.Enums;
using AutoMapper;
using MediatR;

namespace DigitekShop.Application.Features.Inventory.Commands.CreateInventory
{
    public class CreateInventoryCommandHandler : ICommandHandler<CreateInventoryCommand, InventoryDto>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateInventoryCommandHandler(
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InventoryDto> HandleAsync(CreateInventoryCommand command, CancellationToken cancellationToken)
        {
            // Check if product exists
            var product = await _productRepository.GetByIdAsync(command.ProductId);
            if (product == null)
                throw new ProductNotFoundException(command.ProductId);

            // Check if inventory already exists for this product
            var existingInventory = await _inventoryRepository.GetByProductIdAsync(command.ProductId);
            if (existingInventory != null)
                throw new DuplicateEntityException($"Inventory already exists for product {command.ProductId}");

            // Create inventory using full namespace
            var inventory = DigitekShop.Domain.Entities.Inventory.Create(
                command.ProductId,
                command.InitialQuantity,
                command.MinimumStockLevel,
                command.MaximumStockLevel,
                command.Location,
                command.WarehouseCode
            );

            // Save to database
            var createdInventory = await _inventoryRepository.AddAsync(inventory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map to DTO
            var inventoryDto = _mapper.Map<InventoryDto>(createdInventory);
            
            // Set additional properties
            inventoryDto.ProductName = product.Name.Value;
            inventoryDto.ProductSKU = product.SKU.Value;
            inventoryDto.CategoryName = product.Category?.Name.Value;
            inventoryDto.BrandName = product.Brand?.Name.Value;
            inventoryDto.StockUtilizationPercentage = createdInventory.GetStockUtilizationPercentage();
            inventoryDto.IsInStock = createdInventory.IsInStock();
            inventoryDto.IsLowStock = createdInventory.IsLowStock();
            inventoryDto.IsOutOfStock = createdInventory.IsOutOfStock();
            inventoryDto.IsOverstocked = createdInventory.IsOverstocked();

            return inventoryDto;
        }
    }
} 