using System;

namespace DigitekShop.Application.DTOs.Identity
{
    public class AuthAnalyticsDto
    {
        public int TotalLogins { get; set; }
        public int SuccessfulLogins { get; set; }
        public int FailedLogins { get; set; }
        public int ActiveSessions { get; set; }
        public int LockedAccounts { get; set; }
        public int PasswordResets { get; set; }
        public int EmailConfirmations { get; set; }
        public int TwoFactorEnrollments { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastFailedLogin { get; set; }
        public string MostActiveHour { get; set; } = string.Empty;
        public string MostActiveDay { get; set; } = string.Empty;
        public int AverageSessionDuration { get; set; }
        public int UniqueUsersToday { get; set; }
        public int UniqueUsersThisWeek { get; set; }
        public int UniqueUsersThisMonth { get; set; }
    }
} 