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
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : null))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address != null ? src.Address.Province : null))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address != null ? src.Address.Province : null))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address != null ? src.Address.PostalCode : null))
                .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders.Count))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(dest => dest.WishlistCount, opt => opt.MapFrom(src => src.Wishlist != null ? 1 : 0))
                .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.GetTotalSpentAmount()))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.GetAge()))
                .ForMember(dest => dest.IsNewCustomer, opt => opt.MapFrom(src => src.IsNewCustomer()))
                .ForMember(dest => dest.IsVipCustomer, opt => opt.MapFrom(src => src.IsVipCustomer()));

            // DTO to Entity mappings
            CreateMap<CreateCustomerDto, Customer>()
                .ConstructUsing((src, context) => Customer.Create(
                    src.FirstName,
                    src.LastName,
                    new Email(src.Email),
                    new PhoneNumber(src.Phone),
                    src.MiddleName,
                    src.DateOfBirth,
                    src.NationalCode,
                    src.Gender,
                    src.Type));

            CreateMap<UpdateCustomerDto, Customer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
} 