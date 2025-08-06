using DigitekShop.Application.DTOs.Common;
using System.Linq;

namespace DigitekShop.Application.Responses
{
    public static class ResponseFactory
    {
        public static SuccessResponse<T> CreateSuccess<T>(T data, string message = "Operation completed successfully") where T : class
        {
            return SuccessResponse<T>.Create(data, message);
        }

        public static SuccessResponse<T> CreatePagedSuccess<T>(T data, int totalCount, int pageNumber, int pageSize, string message = "Operation completed successfully") where T : class
        {
            return SuccessResponse<T>.CreatePaged(data, totalCount, pageNumber, pageSize, message);
        }

        public static CommandResponse<T> CreateCommand<T>(string operation, string message = "Command executed successfully") where T : class
        {
            return CommandResponse<T>.Create(operation, message);
        }

        public static CommandResponse CreateCommandSuccess(string operation, string message = "Command executed successfully")
        {
            return CommandResponse.Create(operation, message);
        }

        public static CommandResponse<T> CreateCommandWithData<T>(T data, string operation, string message = "Command executed successfully") where T : class
        {
            return CommandResponse<T>.CreateWithData(data, operation, message);
        }

        public static CommandResponse<T> CreateCommandWithId<T>(int id, string operation, string message = "Command executed successfully") where T : class
        {
            return CommandResponse<T>.CreateWithId(id, operation, message);
        }

        public static CommandResponse<T> CreateCommandWithDataAndId<T>(T data, int id, string operation, string message = "Command executed successfully") where T : class
        {
            return CommandResponse<T>.CreateWithDataAndId(data, id, operation, message);
        }

        public static ErrorResponse CreateError(string message, string? errorCode = null)
        {
            return ErrorResponse.Create(message, errorCode);
        }

        public static ErrorResponse CreateValidationError(List<string> errors, string message = "Validation failed")
        {
            return ErrorResponse.CreateValidationError(errors, message);
        }

        public static ErrorResponse CreateValidationError(IEnumerable<FluentValidation.Results.ValidationFailure> validationFailures, string message = "Validation failed")
        {
            var errors = validationFailures.Select(failure => failure.ErrorMessage).ToList();
            return ErrorResponse.CreateValidationError(errors, message);
        }

        public static ErrorResponse CreateNotFound(string entityName, object key)
        {
            return ErrorResponse.CreateNotFound(entityName, key);
        }

        public static ErrorResponse CreateDuplicate(string entityName, string propertyName, object value)
        {
            return ErrorResponse.CreateDuplicate(entityName, propertyName, value);
        }

        public static ErrorResponse CreateBusinessRuleViolation(string message)
        {
            return ErrorResponse.CreateBusinessRuleViolation(message);
        }

        // Helper method to convert from existing ApiResponseDto
        public static SuccessResponse<T> FromApiResponse<T>(ApiResponseDto<T> apiResponse) where T : class
        {
            return new SuccessResponse<T>(apiResponse.Data, apiResponse.Message);
        }

        // Helper method to create response from PagedResultDto
        public static SuccessResponse<List<T>> FromPagedResult<T>(PagedResultDto<T> pagedResult, string message = "Data retrieved successfully") where T : class
        {
            return new SuccessResponse<List<T>>(
                pagedResult.Items,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize,
                message);
        }
    }
} 