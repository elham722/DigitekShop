using System;
using System.Collections.Generic;

namespace DigitekShop.Application.DTOs.Identity
{
    public class PermissionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DisplayName { get; set; }
        public string? Category { get; set; }
        public string? Module { get; set; }
        public string? Resource { get; set; }
        public string? Action { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Computed Properties
        public bool IsSystemPermission { get; set; }
        public int RoleCount { get; set; }
        public int UserCount { get; set; }
        public int ChildPermissionCount { get; set; }
        public int ParentPermissionCount { get; set; }

        // Collections
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<string> Users { get; set; } = new List<string>();
        public IEnumerable<string> ChildPermissions { get; set; } = new List<string>();
        public IEnumerable<string> ParentPermissions { get; set; } = new List<string>();
    }
} 