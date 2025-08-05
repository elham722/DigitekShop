using AutoMapper;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Identity.Profiles
{
    public class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            // CreatePermissionDto -> Permission (when we have a real Permission entity)
            CreateMap<CreatePermissionDto, object>()
                .ForMember(dest => dest, opt => opt.Ignore()); // Placeholder for now

            // UpdatePermissionDto -> Permission (when we have a real Permission entity)
            CreateMap<UpdatePermissionDto, object>()
                .ForMember(dest => dest, opt => opt.Ignore()); // Placeholder for now

            // CreatePermissionCategoryDto -> PermissionCategory (when we have a real entity)
            CreateMap<CreatePermissionCategoryDto, object>()
                .ForMember(dest => dest, opt => opt.Ignore()); // Placeholder for now

            // Note: Since we're using mock data in PermissionService for now,
            // these mappings are placeholders. When we implement real Permission entities,
            // we'll update these mappings accordingly.
        }
    }
} 