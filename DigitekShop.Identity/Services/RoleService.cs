using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DigitekShop.Application.Interfaces.Identity;
using DigitekShop.Identity.Models;
using DigitekShop.Identity.Context;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DigitekShopIdentityDbContext _context;
        private readonly IMapper _mapper;

        public RoleService(
            RoleManager<IdentityRole<string>> roleManager,
            UserManager<ApplicationUser> userManager,
            DigitekShopIdentityDbContext context,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        #region Role CRUD Operations

        public async Task<RoleDto> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return null!;

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return null!;

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<IEnumerable<RoleDto>> GetActiveRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Where(r => !r.Name.StartsWith("Deleted_"))
                .ToListAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<int> GetTotalRolesCountAsync()
        {
            return await _roleManager.Roles.CountAsync();
        }

        public async Task<int> GetActiveRolesCountAsync()
        {
            return await _roleManager.Roles
                .Where(r => !r.Name.StartsWith("Deleted_"))
                .CountAsync();
        }

        #endregion

        #region Role Creation and Management

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            var role = new IdentityRole<string>
            {
                Name = createRoleDto.Name,
                NormalizedName = createRoleDto.Name.ToUpperInvariant()
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> UpdateRoleAsync(string roleId, UpdateRoleDto updateRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return null!;

            if (!string.IsNullOrEmpty(updateRoleDto.Name))
            {
                role.Name = updateRoleDto.Name;
                role.NormalizedName = updateRoleDto.Name.ToUpperInvariant();
            }

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> SoftDeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            role.Name = $"Deleted_{role.Name}_{DateTime.UtcNow:yyyyMMddHHmmss}";
            role.NormalizedName = role.Name.ToUpperInvariant();

            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> RestoreRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            if (role.Name.StartsWith("Deleted_"))
            {
                var originalName = role.Name.Substring(8); // Remove "Deleted_" prefix
                role.Name = originalName;
                role.NormalizedName = originalName.ToUpperInvariant();

                var result = await _roleManager.UpdateAsync(role);
                return result.Succeeded;
            }

            return false;
        }

        #endregion

        #region Role Status Management

        public async Task<bool> ActivateRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            // For now, we'll just return true as Identity doesn't have built-in activation
            // In a real implementation, you might add a custom property to track activation
            return true;
        }

        public async Task<bool> DeactivateRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            // For now, we'll just return true as Identity doesn't have built-in deactivation
            // In a real implementation, you might add a custom property to track deactivation
            return true;
        }

        #endregion

        #region User-Role Management

        public async Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName, int page = 1, int pageSize = 20)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var users = usersInRole
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = roles.ToList();
                userDtos.Add(userDto);
            }

            return userDtos;
        }

        public async Task<int> GetUsersInRoleCountAsync(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.Count;
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> AddUsersToRoleAsync(IEnumerable<string> userIds, string roleName)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }

            return true;
        }

        public async Task<bool> RemoveUsersFromRoleAsync(IEnumerable<string> userIds, string roleName)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }

            return true;
        }

        #endregion

        #region Permission Management

        public async Task<IEnumerable<string>> GetRolePermissionsAsync(string roleName)
        {
            // This would typically come from a separate permissions table
            // For now, we'll return an empty list
            return new List<string>();
        }

        public async Task<bool> AddPermissionToRoleAsync(string roleName, string permission)
        {
            // This would typically add to a separate permissions table
            // For now, we'll return true
            return true;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(string roleName, string permission)
        {
            // This would typically remove from a separate permissions table
            // For now, we'll return true
            return true;
        }

        public async Task<bool> AddPermissionsToRoleAsync(string roleName, IEnumerable<string> permissions)
        {
            // This would typically add to a separate permissions table
            // For now, we'll return true
            return true;
        }

        public async Task<bool> RemovePermissionsFromRoleAsync(string roleName, IEnumerable<string> permissions)
        {
            // This would typically remove from a separate permissions table
            // For now, we'll return true
            return true;
        }

        #endregion

        #region Role Search and Filtering

        public async Task<IEnumerable<RoleDto>> SearchRolesAsync(string searchTerm, int page = 1, int pageSize = 20)
        {
            var roles = await _roleManager.Roles
                .Where(r => r.Name.Contains(searchTerm))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<IEnumerable<RoleDto>> GetRolesByDateRangeAsync(DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 20)
        {
            // IdentityRole doesn't have CreatedAt by default, so we'll return all roles for now
            // In a real implementation, you would need to extend IdentityRole or use a separate table
            var roles = await _roleManager.Roles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        #endregion

        #region Role Analytics

        public async Task<RoleAnalyticsDto> GetRoleAnalyticsAsync()
        {
            var totalRoles = await _roleManager.Roles.CountAsync();
            var activeRoles = await _roleManager.Roles
                .Where(r => !r.Name.StartsWith("Deleted_"))
                .CountAsync();
            var deletedRoles = await _roleManager.Roles
                .Where(r => r.Name.StartsWith("Deleted_"))
                .CountAsync();

            return new RoleAnalyticsDto
            {
                TotalRoles = totalRoles,
                ActiveRoles = activeRoles,
                DeletedRoles = deletedRoles
            };
        }

        public async Task<IEnumerable<RoleActivityDto>> GetRoleActivityAsync(string roleName, int days = 30)
        {
            // This would typically come from a separate activity tracking table
            // For now, we'll return an empty list
            return new List<RoleActivityDto>();
        }

        #endregion

        #region Bulk Operations

        public async Task<bool> BulkActivateRolesAsync(IEnumerable<string> roleIds)
        {
            // For now, we'll just return true
            // In a real implementation, you might update a custom property
            return true;
        }

        public async Task<bool> BulkDeactivateRolesAsync(IEnumerable<string> roleIds)
        {
            // For now, we'll just return true
            // In a real implementation, you might update a custom property
            return true;
        }

        public async Task<bool> BulkDeleteRolesAsync(IEnumerable<string> roleIds)
        {
            var roles = await _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
            foreach (var role in roles)
            {
                await _roleManager.DeleteAsync(role);
            }

            return true;
        }

        #endregion

        #region Validation

        public async Task<bool> IsRoleNameUniqueAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role == null;
        }

        public async Task<bool> IsRoleInUseAsync(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.Any();
        }

        #endregion

    }
} 