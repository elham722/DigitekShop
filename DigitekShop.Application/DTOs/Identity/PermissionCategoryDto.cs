using System;
using System.Collections.Generic;

namespace DigitekShop.Application.DTOs.Identity
{
    public class PermissionCategoryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DisplayName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Computed Properties
        public int PermissionCount { get; set; }
        public int ActivePermissionCount { get; set; }

        // Collections
        public IEnumerable<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
} 