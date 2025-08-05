using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DigitekShop.Application.Interfaces.Identity;
using DigitekShop.Identity.Models;
using DigitekShop.Identity.Context;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DigitekShopIdentityDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, DigitekShopIdentityDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #region User CRUD Operations

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null!;

            var roles = await _userManager.GetRolesAsync(user);
            return MapToUserDto(user, roles);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null!;

            var roles = await _userManager.GetRolesAsync(user);
            return MapToUserDto(user, roles);
        }

        public async Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return null!;

            var roles = await _userManager.GetRolesAsync(user);
            return MapToUserDto(user, roles);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 20)
        {
            var users = await _userManager.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(MapToUserDto(user, roles));
            }

            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync(int page = 1, int pageSize = 20)
        {
            var users = await _userManager.Users
                .Where(u => u.IsActive && !u.IsDeleted)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(MapToUserDto(user, roles));
            }

            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetDeletedUsersAsync(int page = 1, int pageSize = 20)
        {
            var users = await _userManager.Users
                .Where(u => u.IsDeleted)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(MapToUserDto(user, roles));
            }

            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetLockedUsersAsync(int page = 1, int pageSize = 20)
        {
            var users = await _userManager.Users
                .Where(u => u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTime.UtcNow)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(MapToUserDto(user, roles));
            }

            return userDtos;
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _userManager.Users.CountAsync(u => u.IsActive && !u.IsDeleted);
        }

        #endregion

        #region User Creation and Registration

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new ApplicationUser
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                MiddleName = createUserDto.MiddleName,
                DateOfBirth = createUserDto.DateOfBirth,
                Gender = createUserDto.Gender,
                PhoneNumber = createUserDto.PhoneNumber,
                EmailConfirmed = createUserDto.EmailConfirmed,
                PhoneNumberConfirmed = createUserDto.PhoneNumberConfirmed,
                TwoFactorEnabled = createUserDto.TwoFactorEnabled,
                CustomerId = createUserDto.CustomerId,
                CreatedBy = createUserDto.CreatedBy
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Add roles
            if (createUserDto.Roles.Any())
            {
                await _userManager.AddToRolesAsync(user, createUserDto.Roles);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return MapToUserDto(user, roles);
        }

        public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            if (!registerUserDto.AcceptTerms)
            {
                throw new Exception("Terms and conditions must be accepted");
            }

            var createUserDto = new CreateUserDto
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName,
                Password = registerUserDto.Password,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                MiddleName = registerUserDto.MiddleName,
                PhoneNumber = registerUserDto.PhoneNumber,
                DateOfBirth = registerUserDto.DateOfBirth,
                Gender = registerUserDto.Gender,
                CustomerId = registerUserDto.CustomerId,
                Roles = new List<string> { "Customer" } // Default role for registered users
            };

            return await CreateUserAsync(createUserDto);
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        #endregion

        #region User Profile Management

        public async Task<UserDto> UpdateProfileAsync(string userId, UpdateProfileDto updateProfileDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null!;

            user.UpdateProfile(updateProfileDto.FirstName, updateProfileDto.LastName, updateProfileDto.MiddleName);
            user.DateOfBirth = updateProfileDto.DateOfBirth;
            user.Gender = updateProfileDto.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update profile: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return MapToUserDto(user, roles);
        }

        public async Task<UserDto> UpdateContactInfoAsync(string userId, UpdateContactInfoDto updateContactInfoDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null!;

            user.UpdateContactInfo(updateContactInfoDto.Email, updateContactInfoDto.PhoneNumber);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update contact info: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return MapToUserDto(user, roles);
        }



        #endregion

        #region User Status Management

        public async Task<bool> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.Activate();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.Deactivate();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> LockUserAsync(string userId, TimeSpan duration)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.LockAccount(duration);
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UnlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.UnlockAccount();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> SoftDeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.SoftDelete();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> RestoreUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.Restore();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> HardDeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        #endregion

        #region Password Management

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (result.Succeeded)
            {
                user.UpdatePassword();
                await _userManager.UpdateAsync(user);
            }

            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                user.UpdatePassword();
                await _userManager.UpdateAsync(user);
            }

            return result.Succeeded;
        }

        public async Task<bool> ForcePasswordChangeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.LastPasswordChangeAt = DateTime.UtcNow.AddDays(-91); // Force password change
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return string.Empty;

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ResetPasswordWithTokenAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                user.UpdatePassword();
                await _userManager.UpdateAsync(user);
            }

            return result.Succeeded;
        }

        #endregion

        #region Role Management

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> AddUserToRolesAsync(string userId, IEnumerable<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.AddToRolesAsync(user, roleNames);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRolesAsync(string userId, IEnumerable<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.RemoveFromRolesAsync(user, roleNames);
            return result.Succeeded;
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            return await _userManager.IsInRoleAsync(user, roleName);
        }

        #endregion

        #region Two-Factor Authentication

        public async Task<bool> EnableTwoFactorAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.EnableTwoFactor();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DisableTwoFactorAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.DisableTwoFactor();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> IsTwoFactorEnabledAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            return user.TwoFactorEnabled;
        }

        public async Task<string> GenerateTwoFactorSecretAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return string.Empty;

            return await _userManager.GetAuthenticatorKeyAsync(user);
        }

        #endregion

        #region User Search and Filtering

        public async Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20)
        {
            var users = await _userManager.Users
                .Where(u => u.UserName.Contains(searchTerm) || 
                           u.Email.Contains(searchTerm) || 
                           u.FirstName.Contains(searchTerm) || 
                           u.LastName.Contains(searchTerm) ||
                           u.FullName.Contains(searchTerm))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(MapToUserDto(user, roles));
            }

            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName, int page = 1, int pageSize = 20)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var users = usersInRole
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(MapToUserDto(user, roles));
            }

            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByDateRangeAsync(DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 20)
        {
            var users = await _userManager.Users
                .Where(u => u.CreatedAt >= fromDate && u.CreatedAt <= toDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(MapToUserDto(user, roles));
            }

            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByStatusAsync(UserStatus status, int page = 1, int pageSize = 20)
        {
            var users = status switch
            {
                UserStatus.Active => await _userManager.Users.Where(u => u.IsActive && !u.IsDeleted).ToListAsync(),
                UserStatus.Inactive => await _userManager.Users.Where(u => !u.IsActive && !u.IsDeleted).ToListAsync(),
                UserStatus.Locked => await _userManager.Users.Where(u => u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTime.UtcNow).ToListAsync(),
                UserStatus.Deleted => await _userManager.Users.Where(u => u.IsDeleted).ToListAsync(),
                UserStatus.Pending => await _userManager.Users.Where(u => !u.EmailConfirmed).ToListAsync(),
                UserStatus.Suspended => await _userManager.Users.Where(u => !u.IsActive && u.LockoutEnd.HasValue).ToListAsync(),
                _ => await _userManager.Users.ToListAsync()
            };

            var pagedUsers = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var userDtos = new List<UserDto>();
            foreach (var user in pagedUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(MapToUserDto(user, roles));
            }

            return userDtos;
        }

        #endregion

        #region User Analytics

        public async Task<UserAnalyticsDto> GetUserAnalyticsAsync()
        {
            var totalUsers = await _userManager.Users.CountAsync();
            var activeUsers = await _userManager.Users.CountAsync(u => u.IsActive && !u.IsDeleted);
            var inactiveUsers = await _userManager.Users.CountAsync(u => !u.IsActive && !u.IsDeleted);
            var lockedUsers = await _userManager.Users.CountAsync(u => u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTime.UtcNow);
            var deletedUsers = await _userManager.Users.CountAsync(u => u.IsDeleted);
            var newUsersThisMonth = await _userManager.Users.CountAsync(u => u.CreatedAt >= DateTime.UtcNow.AddMonths(-1));
            var newUsersThisWeek = await _userManager.Users.CountAsync(u => u.CreatedAt >= DateTime.UtcNow.AddDays(-7));
            var newUsersToday = await _userManager.Users.CountAsync(u => u.CreatedAt >= DateTime.UtcNow.AddDays(-1));
            var usersWithTwoFactor = await _userManager.Users.CountAsync(u => u.TwoFactorEnabled);
            var usersRequiringPasswordChange = await _userManager.Users.CountAsync(u => u.RequiresPasswordChange());
            var usersWithExpiredPasswords = await _userManager.Users.CountAsync(u => u.IsPasswordExpired());
            var averageLoginAttempts = await _userManager.Users.AverageAsync(u => u.LoginAttempts);
            var lastUserRegistration = await _userManager.Users.MaxAsync(u => u.CreatedAt);
            var lastUserLogin = await _userManager.Users.Where(u => u.LastLoginAt.HasValue).MaxAsync(u => u.LastLoginAt);

            return new UserAnalyticsDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                LockedUsers = lockedUsers,
                DeletedUsers = deletedUsers,
                NewUsersThisMonth = newUsersThisMonth,
                NewUsersThisWeek = newUsersThisWeek,
                NewUsersToday = newUsersToday,
                UsersWithTwoFactor = usersWithTwoFactor,
                UsersRequiringPasswordChange = usersRequiringPasswordChange,
                UsersWithExpiredPasswords = usersWithExpiredPasswords,
                AverageLoginAttempts = averageLoginAttempts,
                LastUserRegistration = lastUserRegistration,
                LastUserLogin = lastUserLogin
            };
        }

        public async Task<IEnumerable<UserActivityDto>> GetUserActivityAsync(string userId, int days = 30)
        {
            // This would typically come from a separate activity tracking table
            // For now, we'll return an empty list
            return new List<UserActivityDto>();
        }

        public async Task<IEnumerable<UserLoginHistoryDto>> GetUserLoginHistoryAsync(string userId, int page = 1, int pageSize = 20)
        {
            // This would typically come from a separate login history table
            // For now, we'll return an empty list
            return new List<UserLoginHistoryDto>();
        }

        #endregion

        #region Bulk Operations

        public async Task<bool> BulkActivateUsersAsync(IEnumerable<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.Activate();
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> BulkDeactivateUsersAsync(IEnumerable<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.Deactivate();
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> BulkDeleteUsersAsync(IEnumerable<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.SoftDelete();
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> BulkAddUsersToRoleAsync(IEnumerable<string> userIds, string roleName)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }

            return true;
        }

        public async Task<bool> BulkRemoveUsersFromRoleAsync(IEnumerable<string> userIds, string roleName)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }

            return true;
        }

        #endregion

        #region Validation

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        public async Task<bool> IsUserNameUniqueAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user == null;
        }



        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return true;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            return user == null;
        }

        #endregion

        #region Security

        public async Task<bool> IncrementLoginAttemptsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.IncrementLoginAttempts();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ResetLoginAttemptsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.ResetLoginAttempts();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdateLastLoginAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.UpdateLastLogin();
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> IsAccountLockedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            return user.IsLocked;
        }

        public async Task<bool> IsPasswordExpiredAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            return user.IsPasswordExpired();
        }

        public async Task<bool> RequiresPasswordChangeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            return user.RequiresPasswordChange();
        }

        #endregion

        #region Private Methods

        private static UserDto MapToUserDto(ApplicationUser user, IEnumerable<string> roles)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                FullName = user.FullName,
                DisplayName = user.DisplayName,
                DateOfBirth = user.DateOfBirth,
                Age = user.Age,
                Gender = user.Gender,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                LastPasswordChangeAt = user.LastPasswordChangeAt,
                LoginAttempts = user.LoginAttempts,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted,
                DeletedAt = user.DeletedAt,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                TwoFactorEnabledAt = user.TwoFactorEnabledAt,
                CustomerId = user.CustomerId,
                CreatedBy = user.CreatedBy,
                UpdatedBy = user.UpdatedBy,
                UpdatedAt = user.UpdatedAt,
                IsLocked = user.IsLocked,
                IsNewUser = user.IsNewUser,
                RequiresPasswordChange = user.RequiresPasswordChange(),
                IsPasswordExpired = user.IsPasswordExpired(),
                Roles = roles,
                Permissions = new List<string>(), // Would be populated from permissions service
                TotalLogins = 0, // Would be populated from login history
                LastActivityAt = user.LastLoginAt,
                LastIpAddress = null, // Would be populated from login history
                LastUserAgent = null // Would be populated from login history
            };
        }

        #endregion
    }
} 