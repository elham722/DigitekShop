using AutoMapper;
using DigitekShop.Domain.Entities;
using DigitekShop.Application.DTOs.Wishlist;

namespace DigitekShop.Application.Profiles
{
    public class WishlistMappingProfile : Profile
    {
        public WishlistMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Wishlist, WishlistItemDto>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : string.Empty))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name.Value : string.Empty))
                .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU.Value : string.Empty))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : string.Empty))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price.Amount : 0))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.AddedAt))
                .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.Product != null && src.Product.IsInStock()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // DTO to Entity mappings
            CreateMap<CreateWishlistItemDto, Wishlist>()
                .ConstructUsing((src, context) => new Wishlist(
                    src.CustomerId,
                    src.ProductId,
                    src.Notes));
        }
    }
} 