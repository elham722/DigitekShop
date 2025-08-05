using AutoMapper;
using DigitekShop.Application.DTOs.Inventory;
using DigitekShop.Domain.Entities;

namespace DigitekShop.Application.Profiles
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            // Inventory to InventoryDto
            CreateMap<Domain.Entities.Inventory, InventoryDto>()
                .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.AvailableQuantity))
                .ForMember(dest => dest.StockUtilizationPercentage, opt => opt.MapFrom(src => src.GetStockUtilizationPercentage()))
                .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.IsInStock()))
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock()))
                .ForMember(dest => dest.IsOutOfStock, opt => opt.MapFrom(src => src.IsOutOfStock()))
                .ForMember(dest => dest.IsOverstocked, opt => opt.MapFrom(src => src.IsOverstocked()));

            // CreateInventoryCommand to Inventory
            CreateMap<Features.Inventory.Commands.CreateInventory.CreateInventoryCommand, Domain.Entities.Inventory>();

            // InventoryTransaction to InventoryTransactionDto
            CreateMap<Domain.Entities.InventoryTransaction, InventoryTransactionDto>()
                .ForMember(dest => dest.QuantityChange, opt => opt.MapFrom(src => src.QuantityChange))
                .ForMember(dest => dest.IsStockIncrease, opt => opt.MapFrom(src => src.IsStockIncrease()))
                .ForMember(dest => dest.IsStockDecrease, opt => opt.MapFrom(src => src.IsStockDecrease()));
        }
    }
} 