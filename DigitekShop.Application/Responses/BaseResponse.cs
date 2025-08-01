using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses
{
    public abstract class BaseResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }

        protected BaseResponse(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }
    }
} 