using System;

namespace DigitekShop.Application.DTOs.Identity
{
    public class UserAnalyticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int LockedUsers { get; set; }
        public int DeletedUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int NewUsersThisWeek { get; set; }
        public int NewUsersToday { get; set; }
        public int UsersWithTwoFactor { get; set; }
        public int UsersRequiringPasswordChange { get; set; }
        public int UsersWithExpiredPasswords { get; set; }
        public double AverageLoginAttempts { get; set; }
        public DateTime? LastUserRegistration { get; set; }
        public DateTime? LastUserLogin { get; set; }
    }
} 