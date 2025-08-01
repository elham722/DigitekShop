using AutoMapper;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Application.DTOs.Product;

namespace DigitekShop.Application.Profiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : string.Empty))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : string.Empty));

            CreateMap<Product, ProductListDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : string.Empty));

            // DTO to Entity mappings
            CreateMap<CreateProductDto, Product>()
                .ConstructUsing((src, context) => new Product(
                    new ProductName(src.Name),
                    src.Description,
                    new Money(src.Price, src.Currency),
                    src.StockQuantity,
                    new SKU(src.SKU),
                    src.CategoryId,
                    src.BrandId,
                    src.Model ?? "",
                    src.Weight))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
} 