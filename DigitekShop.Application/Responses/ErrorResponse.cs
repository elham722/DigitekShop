using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses
{
    public class ErrorResponse : BaseResponse
    {
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("details")]
        public object? Details { get; set; }

        public ErrorResponse(string message, string? errorCode = null) 
            : base(false, message)
        {
            ErrorCode = errorCode;
            Errors.Add(message);
        }

        public ErrorResponse(List<string> errors, string message = "Validation failed", string? errorCode = null) 
            : base(false, message)
        {
            Errors = errors;
            ErrorCode = errorCode;
        }

        public ErrorResponse(string message, object details, string? errorCode = null) 
            : base(false, message)
        {
            ErrorCode = errorCode;
            Details = details;
            Errors.Add(message);
        }

        public static ErrorResponse Create(string message, string? errorCode = null)
        {
            return new ErrorResponse(message, errorCode);
        }

        public static ErrorResponse CreateValidationError(List<string> errors, string message = "Validation failed")
        {
            return new ErrorResponse(errors, message, "VALIDATION_ERROR");
        }

        public static ErrorResponse CreateNotFound(string entityName, object key)
        {
            return new ErrorResponse($"{entityName} with ID {key} was not found", "NOT_FOUND");
        }

        public static ErrorResponse CreateDuplicate(string entityName, string propertyName, object value)
        {
            return new ErrorResponse($"{entityName} with {propertyName} '{value}' already exists", "DUPLICATE_ENTITY");
        }

        public static ErrorResponse CreateBusinessRuleViolation(string message)
        {
            return new ErrorResponse(message, "BUSINESS_RULE_VIOLATION");
        }
    }
} 