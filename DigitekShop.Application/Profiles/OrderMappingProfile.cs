using AutoMapper;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Application.DTOs.Order;

namespace DigitekShop.Application.Profiles
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber.Value))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Amount))
                .ForMember(dest => dest.ShippingCost, opt => opt.MapFrom(src => src.ShippingCost.Amount))
                .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount.Amount))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount.Amount))
                .ForMember(dest => dest.FinalAmount, opt => opt.MapFrom(src => src.FinalAmount.Amount))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : string.Empty))
                .ForMember(dest => dest.ShippedDate, opt => opt.MapFrom(src => src.ShippedAt))
                .ForMember(dest => dest.DeliveredDate, opt => opt.MapFrom(src => src.DeliveredAt));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name.Value : string.Empty))
                .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU.Value : string.Empty))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : string.Empty));

            // Address mappings
            CreateMap<Address, AddressDto>()
                .ReverseMap();

            // DTO to Entity mappings (for updates)
            CreateMap<UpdateOrderDto, Order>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
} 