using AutoMapper;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Application.DTOs.Customer;

namespace DigitekShop.Application.Profiles
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value))
                .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders.Count))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(dest => dest.WishlistCount, opt => opt.MapFrom(src => src.Wishlists.Count))
                .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.GetTotalSpent()));

            // DTO to Entity mappings
            CreateMap<CreateCustomerDto, Customer>()
                .ConstructUsing((src, context) => new Customer(
                    src.FirstName,
                    src.LastName,
                    src.DateOfBirth,
                    src.NationalCode ?? "",
                    new Email(src.Email),
                    new PhoneNumber(src.Phone),
                    src.ProfileImageUrl,
                    src.Notes));

            CreateMap<UpdateCustomerDto, Customer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
} 