using System;

namespace DigitekShop.Application.DTOs.Identity
{
    public class PermissionAnalyticsDto
    {
        public int TotalPermissions { get; set; }
        public int ActivePermissions { get; set; }
        public int DeletedPermissions { get; set; }
        public int SystemPermissions { get; set; }
        public int CustomPermissions { get; set; }
        public int PermissionsWithRoles { get; set; }
        public int PermissionsWithUsers { get; set; }
        public int EmptyPermissions { get; set; }
        public DateTime LastPermissionCreated { get; set; }
        public DateTime LastPermissionUpdated { get; set; }
        public string MostUsedPermission { get; set; } = string.Empty;
        public int MostUsedPermissionCount { get; set; }
        public int PermissionCategories { get; set; }
        public int PermissionModules { get; set; }
    }
} 