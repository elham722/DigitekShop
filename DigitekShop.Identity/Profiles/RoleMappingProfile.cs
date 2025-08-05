using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Identity.Profiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            // IdentityRole<string> -> RoleDto
            CreateMap<IdentityRole<string>, RoleDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => 
                    !src.Name.StartsWith("Deleted_")))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => 
                    src.Name.StartsWith("Deleted_")))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsSystemRole, opt => opt.MapFrom(src => 
                    IsSystemRole(src.Name)))
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.PermissionCount, opt => opt.MapFrom(src => 0));

            // CreateRoleDto -> IdentityRole<string>
            CreateMap<CreateRoleDto, IdentityRole<string>>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpperInvariant()))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            // UpdateRoleDto -> IdentityRole<string> (partial update)
            CreateMap<UpdateRoleDto, IdentityRole<string>>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpperInvariant()))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        }

        private static bool IsSystemRole(string roleName)
        {
            var systemRoles = new[] { "Admin", "Customer", "Manager", "Support" };
            return systemRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
        }
    }
} 