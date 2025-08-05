using System;

namespace DigitekShop.Application.DTOs.Identity
{
    public class RoleAnalyticsDto
    {
        public int TotalRoles { get; set; }
        public int ActiveRoles { get; set; }
        public int DeletedRoles { get; set; }
        public int SystemRoles { get; set; }
        public int CustomRoles { get; set; }
        public int RolesWithUsers { get; set; }
        public int EmptyRoles { get; set; }
        public DateTime LastRoleCreated { get; set; }
        public DateTime LastRoleUpdated { get; set; }
        public string MostUsedRole { get; set; } = string.Empty;
        public int MostUsedRoleCount { get; set; }
    }
} 