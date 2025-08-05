using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Interfaces.Identity
{
    public interface IUserService
    {
        // User CRUD Operations
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByUserNameAsync(string userName);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 20);
        Task<IEnumerable<UserDto>> GetActiveUsersAsync(int page = 1, int pageSize = 20);
        Task<IEnumerable<UserDto>> GetDeletedUsersAsync(int page = 1, int pageSize = 20);
        Task<IEnumerable<UserDto>> GetLockedUsersAsync(int page = 1, int pageSize = 20);
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetActiveUsersCountAsync();

        // User Creation and Registration
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<bool> ConfirmEmailAsync(string userId, string token);

        // User Profile Management
        Task<UserDto> UpdateProfileAsync(string userId, UpdateProfileDto updateProfileDto);
        Task<UserDto> UpdateContactInfoAsync(string userId, UpdateContactInfoDto updateContactInfoDto);

        // User Status Management
        Task<bool> ActivateUserAsync(string userId);
        Task<bool> DeactivateUserAsync(string userId);
        Task<bool> LockUserAsync(string userId, TimeSpan duration);
        Task<bool> UnlockUserAsync(string userId);
        Task<bool> SoftDeleteUserAsync(string userId);
        Task<bool> RestoreUserAsync(string userId);
        Task<bool> HardDeleteUserAsync(string userId);

        // Password Management
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<bool> ResetPasswordAsync(string userId, string newPassword);
        Task<bool> ForcePasswordChangeAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordWithTokenAsync(string email, string token, string newPassword);

        // User-Role Management (Read-only)
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);

        // Two-Factor Authentication
        Task<bool> EnableTwoFactorAsync(string userId);
        Task<bool> DisableTwoFactorAsync(string userId);
        Task<bool> IsTwoFactorEnabledAsync(string userId);
        Task<string> GenerateTwoFactorSecretAsync(string userId);

        // User Search and Filtering
        Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName, int page = 1, int pageSize = 20);
        Task<IEnumerable<UserDto>> GetUsersByDateRangeAsync(DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 20);
        Task<IEnumerable<UserDto>> GetUsersByStatusAsync(UserStatus status, int page = 1, int pageSize = 20);

        // User Analytics
        Task<UserAnalyticsDto> GetUserAnalyticsAsync();
        Task<IEnumerable<UserActivityDto>> GetUserActivityAsync(string userId, int days = 30);
        Task<IEnumerable<UserLoginHistoryDto>> GetUserLoginHistoryAsync(string userId, int page = 1, int pageSize = 20);

        // Bulk Operations
        Task<bool> BulkActivateUsersAsync(IEnumerable<string> userIds);
        Task<bool> BulkDeactivateUsersAsync(IEnumerable<string> userIds);
        Task<bool> BulkDeleteUsersAsync(IEnumerable<string> userIds);
        Task<bool> BulkAddUsersToRoleAsync(IEnumerable<string> userIds, string roleName);
        Task<bool> BulkRemoveUsersFromRoleAsync(IEnumerable<string> userIds, string roleName);

        // Validation
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsUserNameUniqueAsync(string userName);
        Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber);

        // Security
        Task<bool> IncrementLoginAttemptsAsync(string userId);
        Task<bool> ResetLoginAttemptsAsync(string userId);
        Task<bool> UpdateLastLoginAsync(string userId);
        Task<bool> IsAccountLockedAsync(string userId);
        Task<bool> IsPasswordExpiredAsync(string userId);
        Task<bool> RequiresPasswordChangeAsync(string userId);
    }
} 