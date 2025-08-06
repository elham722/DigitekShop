using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses.Identity
{
    public class AuthResponse : BaseResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("expiresAt")]
        public DateTime? ExpiresAt { get; set; }

        [JsonPropertyName("requiresTwoFactor")]
        public bool RequiresTwoFactor { get; set; }

        [JsonPropertyName("twoFactorToken")]
        public string? TwoFactorToken { get; set; }

        [JsonPropertyName("user")]
        public object? User { get; set; }

        public AuthResponse(bool success, string message = "") : base(success, message)
        {
        }

        public static AuthResponse CreateSuccess(string token, DateTime expiresAt, object user, string message = "Authentication successful")
        {
            return new AuthResponse(true, message)
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = user
            };
        }

        public static AuthResponse CreateTwoFactorRequired(string twoFactorToken, string message = "Two-factor authentication required")
        {
            return new AuthResponse(false, message)
            {
                RequiresTwoFactor = true,
                TwoFactorToken = twoFactorToken
            };
        }

        public static AuthResponse CreateError(string message, string? errorCode = null)
        {
            return new AuthResponse(false, message);
        }
    }
} 