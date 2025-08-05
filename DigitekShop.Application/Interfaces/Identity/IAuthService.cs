using System;
using System.Threading.Tasks;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        // Authentication
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> LoginWithTwoFactorAsync(LoginWithTwoFactorDto loginDto);
        Task<bool> LogoutAsync(string userId);
        Task<bool> LogoutAllDevicesAsync(string userId);

        // Registration
        Task<RegistrationResponseDto> RegisterAsync(RegisterUserDto registerUserDto);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<bool> ResendEmailConfirmationAsync(string email);

        // Password Management
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<bool> ForcePasswordChangeAsync(string userId);

        // Two-Factor Authentication
        Task<bool> EnableTwoFactorAsync(string userId);
        Task<bool> DisableTwoFactorAsync(string userId);
        Task<string> GenerateTwoFactorSecretAsync(string userId);
        Task<bool> VerifyTwoFactorCodeAsync(string userId, string code);

        // Account Management
        Task<bool> LockAccountAsync(string userId, TimeSpan duration);
        Task<bool> UnlockAccountAsync(string userId);
        Task<bool> ActivateAccountAsync(string userId);
        Task<bool> DeactivateAccountAsync(string userId);

        // Session Management
        Task<bool> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<IEnumerable<SessionDto>> GetUserSessionsAsync(string userId);
        Task<bool> RevokeSessionAsync(string userId, string sessionId);

        // Security
        Task<bool> ValidatePasswordAsync(string password);
        Task<bool> CheckPasswordStrengthAsync(string password);
        Task<bool> IsAccountLockedAsync(string userId);
        Task<bool> IsPasswordExpiredAsync(string userId);
        Task<bool> RequiresPasswordChangeAsync(string userId);

        // Analytics
        Task<AuthAnalyticsDto> GetAuthAnalyticsAsync();
        Task<IEnumerable<LoginHistoryDto>> GetLoginHistoryAsync(string userId, int page = 1, int pageSize = 20);
    }
}
