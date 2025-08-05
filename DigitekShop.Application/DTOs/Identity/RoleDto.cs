using System;
using System.Collections.Generic;

namespace DigitekShop.Application.DTOs.Identity
{
    public class RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? NormalizedName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Computed Properties
        public bool IsSystemRole { get; set; }
        public int UserCount { get; set; }
        public int PermissionCount { get; set; }

        // Collections
        public IEnumerable<string> Permissions { get; set; } = new List<string>();
        public IEnumerable<string> Users { get; set; } = new List<string>();
    }
} 