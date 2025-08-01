using AutoMapper;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Application.DTOs.Brand;

namespace DigitekShop.Application.Profiles
{
    public class BrandMappingProfile : Profile
    {
        public BrandMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Brand, BrandDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));

            // DTO to Entity mappings
            CreateMap<CreateBrandDto, Brand>()
                .ConstructUsing((src, context) => new Brand(
                    new ProductName(src.Name),
                    src.Description,
                    src.LogoUrl,
                    src.Website));

            CreateMap<UpdateBrandDto, Brand>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
} 