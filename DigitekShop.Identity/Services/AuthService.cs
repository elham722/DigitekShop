
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DigitekShop.Application.Interfaces.Identity;
using DigitekShop.Identity.Models;
using DigitekShop.Identity.Context;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly DigitekShopIdentityDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<string>> roleManager,
            DigitekShopIdentityDbContext context,
            IConfiguration configuration,
            IUserService userService,
            IPermissionService permissionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _userService = userService;
            _permissionService = permissionService;
        }

        #region Login & Authentication

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Invalid email or password"
                };
            }

            if (!user.IsActive)
            {
                return new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Account is deactivated"
                };
            }

            if (user.IsLocked)
            {
                return new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Account is locked"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            if (!result.Succeeded)
            {
                await _userService.IncrementLoginAttemptsAsync(user.Id);
                return new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Invalid email or password"
                };
            }

            if (result.RequiresTwoFactor)
            {
                return new AuthResponseDto
                {
                    Succeeded = false,
                    RequiresTwoFactor = true,
                    Message = "Two-factor authentication required"
                };
            }

            await _userService.ResetLoginAttemptsAsync(user.Id);
            await _userService.UpdateLastLoginAsync(user.Id);

            var token = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            return new AuthResponseDto
            {
                Succeeded = true,
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = await _userService.GetUserByIdAsync(user.Id),
                Roles = await _userService.GetUserRolesAsync(user.Id),
                Permissions = (await _permissionService.GetUserPermissionsAsync(user.Id)).Select(p => p.Name),
                LastLoginAt = user.LastLoginAt
            };
        }

        public async Task<AuthResponseDto> LoginWithTwoFactorAsync(LoginWithTwoFactorDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Invalid email or password"
                };
            }

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                loginDto.TwoFactorCode, loginDto.RememberMe, false);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Invalid two-factor code"
                };
            }

            await _userService.ResetLoginAttemptsAsync(user.Id);
            await _userService.UpdateLastLoginAsync(user.Id);

            var token = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            return new AuthResponseDto
            {
                Succeeded = true,
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = await _userService.GetUserByIdAsync(user.Id),
                Roles = await _userService.GetUserRolesAsync(user.Id),
                Permissions = (await _permissionService.GetUserPermissionsAsync(user.Id)).Select(p => p.Name),
                LastLoginAt = user.LastLoginAt
            };
        }

        public async Task<bool> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            await _signInManager.SignOutAsync();
            return true;
        }

        #endregion

        #region Registration

        public async Task<RegistrationResponseDto> RegisterAsync(RegisterUserDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new RegistrationResponseDto
                {
                    Succeeded = false,
                    Message = "Email already exists",
                    Errors = new List<string> { "Email already exists" }
                };
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return new RegistrationResponseDto
                {
                    Succeeded = false,
                    Message = "Registration failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // Add default role if specified
            if (!string.IsNullOrEmpty(registerDto.DefaultRole))
            {
                await _userManager.AddToRoleAsync(user, registerDto.DefaultRole);
            }

            // Generate email confirmation token
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return new RegistrationResponseDto
            {
                Succeeded = true,
                Message = "Registration successful",
                UserId = user.Id,
                EmailConfirmationToken = emailConfirmationToken,
                RequiresEmailConfirmation = true,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        #endregion

        #region Password Management

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null) return false;

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            return result.Succeeded;
        }





        public async Task<bool> ForcePasswordChangeAsync(string userId)
        {
            return await _userService.ForcePasswordChangeAsync(userId);
        }

        #endregion

        #region Two-Factor Authentication

        public async Task<bool> EnableTwoFactorAsync(string userId)
        {
            return await _userService.EnableTwoFactorAsync(userId);
        }

        public async Task<bool> DisableTwoFactorAsync(string userId)
        {
            return await _userService.DisableTwoFactorAsync(userId);
        }

        public async Task<bool> IsTwoFactorEnabledAsync(string userId)
        {
            return await _userService.IsTwoFactorEnabledAsync(userId);
        }

        public async Task<string> GenerateTwoFactorSecretAsync(string userId)
        {
            return await _userService.GenerateTwoFactorSecretAsync(userId);
        }

        #endregion

        #region Account Management

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<bool> LockAccountAsync(string userId, TimeSpan duration)
        {
            return await _userService.LockUserAsync(userId, duration);
        }

        public async Task<bool> UnlockAccountAsync(string userId)
        {
            return await _userService.UnlockUserAsync(userId);
        }

        public async Task<bool> DeactivateAccountAsync(string userId)
        {
            return await _userService.DeactivateUserAsync(userId);
        }

        public async Task<bool> ActivateAccountAsync(string userId)
        {
            return await _userService.ActivateUserAsync(userId);
        }

        #endregion

        #region Session Management

        public async Task<IEnumerable<SessionDto>> GetUserSessionsAsync(string userId)
        {
            // This would typically come from a separate sessions table
            // For now, we'll return mock sessions
            return new List<SessionDto>
            {
                new SessionDto
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    DeviceName = "Chrome on Windows",
                    IpAddress = "192.168.1.100",
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    LastActivityAt = DateTime.UtcNow.AddMinutes(-5),
                    ExpiresAt = DateTime.UtcNow.AddHours(22),
                    IsActive = true,
                    Location = "Tehran, Iran",
                    Browser = "Chrome",
                    OperatingSystem = "Windows 10"
                }
            };
        }

        public async Task<bool> RevokeSessionAsync(string userId, string sessionId)
        {
            // This would typically remove the specific session for the user
            // For now, we'll return true
            return true;
        }

        public async Task<bool> LogoutAllDevicesAsync(string userId)
        {
            // This would typically remove all sessions for the user
            // For now, we'll return true
            return true;
        }

        #endregion

        #region Security

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RefreshTokenAsync(string refreshToken)
        {
            // This would typically validate the refresh token and generate a new access token
            // For now, we'll return true to indicate success
            return true;
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            // This would typically invalidate the refresh token
            // For now, we'll return true
            return true;
        }

        #endregion

        #region Missing Methods

        public async Task<bool> ResendEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // This would typically send the email
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // This would typically send the email
            return true;
        }

        public async Task<bool> VerifyTwoFactorCodeAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, code);
            return result;
        }

        public async Task<bool> ValidatePasswordAsync(string password)
        {
            var validators = _userManager.PasswordValidators;
            var user = new ApplicationUser();
            
            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(_userManager, user, password);
                if (!result.Succeeded)
                    return false;
            }
            
            return true;
        }

        public async Task<bool> CheckPasswordStrengthAsync(string password)
        {
            // This would typically check password strength
            // For now, we'll return true if password is at least 6 characters
            return password.Length >= 6;
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

        #region Analytics

        public async Task<AuthAnalyticsDto> GetAuthAnalyticsAsync()
        {
            // This would typically come from a separate analytics table
            // For now, we'll return mock analytics
            return new AuthAnalyticsDto
            {
                TotalLogins = 150,
                SuccessfulLogins = 145,
                FailedLogins = 5,
                ActiveSessions = 25,
                LockedAccounts = 2,
                PasswordResets = 8,
                EmailConfirmations = 12,
                TwoFactorEnrollments = 3,
                LastLogin = DateTime.UtcNow.AddMinutes(-30),
                LastFailedLogin = DateTime.UtcNow.AddHours(-2),
                MostActiveHour = "14:00",
                MostActiveDay = "Monday",
                AverageSessionDuration = 45,
                UniqueUsersToday = 45,
                UniqueUsersThisWeek = 120,
                UniqueUsersThisMonth = 450
            };
        }

        public async Task<IEnumerable<LoginHistoryDto>> GetLoginHistoryAsync(string userId, int page = 1, int pageSize = 20)
        {
            // This would typically come from a separate login history table
            // For now, we'll return mock history
            return new List<LoginHistoryDto>
            {
                new LoginHistoryDto
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    LoginAt = DateTime.UtcNow.AddHours(-2),
                    LogoutAt = DateTime.UtcNow.AddHours(-1),
                    IsSuccessful = true,
                    IpAddress = "192.168.1.100",
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                    Location = "Tehran, Iran",
                    Browser = "Chrome",
                    OperatingSystem = "Windows 10",
                    DeviceType = "Desktop",
                    IsTwoFactor = false
                }
            };
        }

        #endregion

        #region Private Methods

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);

            var userRoles = await _userManager.GetRolesAsync(user);
            var userPermissions = await _permissionService.GetUserPermissionsAsync(user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("FirstName", user.FirstName ?? string.Empty),
                new Claim("LastName", user.LastName ?? string.Empty),
                new Claim("CustomerId", user.CustomerId?.ToString() ?? string.Empty)
            };

            // Add roles to claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add permissions to claims
            foreach (var permission in userPermissions)
            {
                claims.Add(new Claim("Permission", permission.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        #endregion
    }
}
