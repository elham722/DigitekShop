using AutoMapper;
using DigitekShop.Application.DTOs.Product;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Application.Profiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Product -> ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.GetAverageRating().Value))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.GetReviewCount()));

            // CreateProductDto -> Product
            CreateMap<CreateProductDto, Product>()
                .ConstructUsing((src, context) => Product.Create(
                    new ProductName(src.Name),
                    src.Description,
                    new Money(src.Price, "IRR"),
                    src.StockQuantity,
                    new SKU(src.SKU),
                    src.CategoryId,
                    src.BrandId,
                    src.Model,
                    src.Weight
                ));

            // UpdateProductDto -> Product (partial update)
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.Name) ? null : new ProductName(src.Name)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => 
                    !src.Price.HasValue ? null : new Money(src.Price.Value, "IRR")))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.SKU) ? null : new SKU(src.SKU)));

            // ProductListDto mapping
            CreateMap<Product, ProductListDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU.Value))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.GetAverageRating().Value))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.GetReviewCount()));
        }
    }
} 