using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Interfaces.Identity
{
    public interface IRoleService
    {
        // Role CRUD Operations
        Task<RoleDto> GetRoleByIdAsync(string roleId);
        Task<RoleDto> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<IEnumerable<RoleDto>> GetActiveRolesAsync();
        Task<int> GetTotalRolesCountAsync();
        Task<int> GetActiveRolesCountAsync();

        // Role Creation and Management
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<RoleDto> UpdateRoleAsync(string roleId, UpdateRoleDto updateRoleDto);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<bool> SoftDeleteRoleAsync(string roleId);
        Task<bool> RestoreRoleAsync(string roleId);

        // Role Status Management
        Task<bool> ActivateRoleAsync(string roleId);
        Task<bool> DeactivateRoleAsync(string roleId);

        // User-Role Management
        Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName, int page = 1, int pageSize = 20);
        Task<int> GetUsersInRoleCountAsync(string roleName);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<bool> AddUsersToRoleAsync(IEnumerable<string> userIds, string roleName);
        Task<bool> RemoveUsersFromRoleAsync(IEnumerable<string> userIds, string roleName);

        // Permission Management
        Task<IEnumerable<string>> GetRolePermissionsAsync(string roleName);
        Task<bool> AddPermissionToRoleAsync(string roleName, string permission);
        Task<bool> RemovePermissionFromRoleAsync(string roleName, string permission);
        Task<bool> AddPermissionsToRoleAsync(string roleName, IEnumerable<string> permissions);
        Task<bool> RemovePermissionsFromRoleAsync(string roleName, IEnumerable<string> permissions);

        // Role Search and Filtering
        Task<IEnumerable<RoleDto>> SearchRolesAsync(string searchTerm, int page = 1, int pageSize = 20);
        Task<IEnumerable<RoleDto>> GetRolesByDateRangeAsync(DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 20);

        // Role Analytics
        Task<RoleAnalyticsDto> GetRoleAnalyticsAsync();
        Task<IEnumerable<RoleActivityDto>> GetRoleActivityAsync(string roleName, int days = 30);

        // Bulk Operations
        Task<bool> BulkActivateRolesAsync(IEnumerable<string> roleIds);
        Task<bool> BulkDeactivateRolesAsync(IEnumerable<string> roleIds);
        Task<bool> BulkDeleteRolesAsync(IEnumerable<string> roleIds);

        // Validation
        Task<bool> IsRoleNameUniqueAsync(string roleName);
        Task<bool> IsRoleInUseAsync(string roleName);
    }
} 