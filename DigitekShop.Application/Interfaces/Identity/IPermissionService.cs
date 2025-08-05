using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Interfaces.Identity
{
    public interface IPermissionService
    {
        // Permission CRUD Operations
        Task<PermissionDto> GetPermissionByIdAsync(string permissionId);
        Task<PermissionDto> GetPermissionByNameAsync(string permissionName);
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        Task<IEnumerable<PermissionDto>> GetActivePermissionsAsync();
        Task<int> GetTotalPermissionsCountAsync();
        Task<int> GetActivePermissionsCountAsync();

        // Permission Creation and Management
        Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto createPermissionDto);
        Task<PermissionDto> UpdatePermissionAsync(string permissionId, UpdatePermissionDto updatePermissionDto);
        Task<bool> DeletePermissionAsync(string permissionId);
        Task<bool> SoftDeletePermissionAsync(string permissionId);
        Task<bool> RestorePermissionAsync(string permissionId);

        // Permission Status Management
        Task<bool> ActivatePermissionAsync(string permissionId);
        Task<bool> DeactivatePermissionAsync(string permissionId);

        // Role-Permission Management
        Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(string roleName);
        Task<bool> AddPermissionToRoleAsync(string roleName, string permissionName);
        Task<bool> RemovePermissionFromRoleAsync(string roleName, string permissionName);
        Task<bool> AddPermissionsToRoleAsync(string roleName, IEnumerable<string> permissionNames);
        Task<bool> RemovePermissionsFromRoleAsync(string roleName, IEnumerable<string> permissionNames);

        // User-Permission Management
        Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(string userId);
        Task<bool> AddPermissionToUserAsync(string userId, string permissionName);
        Task<bool> RemovePermissionFromUserAsync(string userId, string permissionName);
        Task<bool> AddPermissionsToUserAsync(string userId, IEnumerable<string> permissionNames);
        Task<bool> RemovePermissionsFromUserAsync(string userId, IEnumerable<string> permissionNames);

        // Permission Categories
        Task<IEnumerable<PermissionCategoryDto>> GetPermissionCategoriesAsync();
        Task<IEnumerable<PermissionDto>> GetPermissionsByCategoryAsync(string category);
        Task<bool> CreatePermissionCategoryAsync(CreatePermissionCategoryDto createCategoryDto);

        // Permission Search and Filtering
        Task<IEnumerable<PermissionDto>> SearchPermissionsAsync(string searchTerm, int page = 1, int pageSize = 20);
        Task<IEnumerable<PermissionDto>> GetPermissionsByDateRangeAsync(DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 20);
        Task<IEnumerable<PermissionDto>> GetPermissionsByModuleAsync(string module, int page = 1, int pageSize = 20);

        // Permission Analytics
        Task<PermissionAnalyticsDto> GetPermissionAnalyticsAsync();
        Task<IEnumerable<PermissionActivityDto>> GetPermissionActivityAsync(string permissionName, int days = 30);

        // Bulk Operations
        Task<bool> BulkActivatePermissionsAsync(IEnumerable<string> permissionIds);
        Task<bool> BulkDeactivatePermissionsAsync(IEnumerable<string> permissionIds);
        Task<bool> BulkDeletePermissionsAsync(IEnumerable<string> permissionIds);

        // Validation
        Task<bool> IsPermissionNameUniqueAsync(string permissionName);
        Task<bool> IsPermissionInUseAsync(string permissionName);
        Task<bool> HasPermissionAsync(string userId, string permissionName);
        Task<bool> HasAnyPermissionAsync(string userId, IEnumerable<string> permissionNames);
        Task<bool> HasAllPermissionsAsync(string userId, IEnumerable<string> permissionNames);

        // Permission Hierarchy
        Task<IEnumerable<PermissionDto>> GetParentPermissionsAsync(string permissionName);
        Task<IEnumerable<PermissionDto>> GetChildPermissionsAsync(string permissionName);
        Task<bool> AddChildPermissionAsync(string parentPermissionName, string childPermissionName);
        Task<bool> RemoveChildPermissionAsync(string parentPermissionName, string childPermissionName);
    }
} 