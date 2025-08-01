using AutoMapper;
using DigitekShop.Domain.Entities;
using DigitekShop.Application.DTOs.Review;

namespace DigitekShop.Application.Profiles
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name.Value : string.Empty))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : string.Empty));

            // DTO to Entity mappings
            CreateMap<CreateReviewDto, Review>()
                .ConstructUsing((src, context) => new Review(
                    src.ProductId,
                    src.CustomerId,
                    src.Rating,
                    src.Title,
                    src.Comment,
                    src.IsVerifiedPurchase));
        }
    }
} 