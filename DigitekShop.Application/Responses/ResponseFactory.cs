using DigitekShop.Application.DTOs.Common;
using System.Linq;
using DigitekShop.Application.Responses.Identity;
using DigitekShop.Application.Responses.Analytics;
using DigitekShop.Application.Responses.File;
using DigitekShop.Application.Responses.Email;

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

        // Identity Response Factory Methods
        public static AuthResponse CreateAuthSuccess(string token, DateTime expiresAt, object user, string message = "Authentication successful")
        {
            return AuthResponse.CreateSuccess(token, expiresAt, user, message);
        }

        public static AuthResponse CreateAuthTwoFactorRequired(string twoFactorToken, string message = "Two-factor authentication required")
        {
            return AuthResponse.CreateTwoFactorRequired(twoFactorToken, message);
        }

        public static AuthResponse CreateAuthError(string message, string? errorCode = null)
        {
            return AuthResponse.CreateError(message, errorCode);
        }

        public static UserResponse CreateUserSuccess(object user, string message = "User operation successful")
        {
            return UserResponse.CreateSuccess(user, message);
        }

        public static UserResponse CreateUserPagedSuccess(object users, int totalCount, int pageNumber, int pageSize, string message = "Users retrieved successfully")
        {
            return UserResponse.CreatePagedSuccess(users, totalCount, pageNumber, pageSize, message);
        }

        public static UserResponse CreateUserWithRoles(object user, IEnumerable<string> roles, string message = "User with roles retrieved successfully")
        {
            return UserResponse.CreateWithRoles(user, roles, message);
        }

        public static UserResponse CreateUserWithPermissions(object user, IEnumerable<string> permissions, string message = "User with permissions retrieved successfully")
        {
            return UserResponse.CreateWithPermissions(user, permissions, message);
        }

        public static UserResponse CreateUserError(string message, string? errorCode = null)
        {
            return UserResponse.CreateError(message, errorCode);
        }

        public static RoleResponse CreateRoleSuccess(object role, string message = "Role operation successful")
        {
            return RoleResponse.CreateSuccess(role, message);
        }

        public static RoleResponse CreateRolePagedSuccess(object roles, int totalCount, int pageNumber, int pageSize, string message = "Roles retrieved successfully")
        {
            return RoleResponse.CreatePagedSuccess(roles, totalCount, pageNumber, pageSize, message);
        }

        public static RoleResponse CreateRoleWithPermissions(object role, IEnumerable<string> permissions, string message = "Role with permissions retrieved successfully")
        {
            return RoleResponse.CreateWithPermissions(role, permissions, message);
        }

        public static RoleResponse CreateRoleWithUsers(object role, IEnumerable<string> users, string message = "Role with users retrieved successfully")
        {
            return RoleResponse.CreateWithUsers(role, users, message);
        }

        public static RoleResponse CreateRoleError(string message, string? errorCode = null)
        {
            return RoleResponse.CreateError(message, errorCode);
        }

        public static PermissionResponse CreatePermissionSuccess(object permission, string message = "Permission operation successful")
        {
            return PermissionResponse.CreateSuccess(permission, message);
        }

        public static PermissionResponse CreatePermissionPagedSuccess(object permissions, int totalCount, int pageNumber, int pageSize, string message = "Permissions retrieved successfully")
        {
            return PermissionResponse.CreatePagedSuccess(permissions, totalCount, pageNumber, pageSize, message);
        }

        public static PermissionResponse CreatePermissionWithRoles(object permission, IEnumerable<string> roles, string message = "Permission with roles retrieved successfully")
        {
            return PermissionResponse.CreateWithRoles(permission, roles, message);
        }

        public static PermissionResponse CreatePermissionWithUsers(object permission, IEnumerable<string> users, string message = "Permission with users retrieved successfully")
        {
            return PermissionResponse.CreateWithUsers(permission, users, message);
        }

        public static PermissionResponse CreatePermissionError(string message, string? errorCode = null)
        {
            return PermissionResponse.CreateError(message, errorCode);
        }

        // Analytics Response Factory Methods
        public static AnalyticsResponse CreateAnalyticsSuccess(object analytics, string period, DateTime startDate, DateTime endDate, string message = "Analytics retrieved successfully")
        {
            return AnalyticsResponse.CreateSuccess(analytics, period, startDate, endDate, message);
        }

        public static AnalyticsResponse CreateAnalyticsWithMetrics(object analytics, Dictionary<string, object> metrics, string message = "Analytics with metrics retrieved successfully")
        {
            return AnalyticsResponse.CreateWithMetrics(analytics, metrics, message);
        }

        public static AnalyticsResponse CreateAnalyticsWithTrends(object analytics, object trends, string message = "Analytics with trends retrieved successfully")
        {
            return AnalyticsResponse.CreateWithTrends(analytics, trends, message);
        }

        public static AnalyticsResponse CreateAnalyticsError(string message, string? errorCode = null)
        {
            return AnalyticsResponse.CreateError(message, errorCode);
        }

        public static ActivityResponse CreateActivitySuccess(object activities, string message = "Activities retrieved successfully")
        {
            return ActivityResponse.CreateSuccess(activities, message);
        }

        public static ActivityResponse CreateActivityPagedSuccess(object activities, int totalCount, int pageNumber, int pageSize, string message = "Activities retrieved successfully")
        {
            return ActivityResponse.CreatePagedSuccess(activities, totalCount, pageNumber, pageSize, message);
        }

        public static ActivityResponse CreateActivityFiltered(object activities, string activityType, string userId, DateTime startDate, DateTime endDate, string message = "Filtered activities retrieved successfully")
        {
            return ActivityResponse.CreateFiltered(activities, activityType, userId, startDate, endDate, message);
        }

        public static ActivityResponse CreateActivityError(string message, string? errorCode = null)
        {
            return ActivityResponse.CreateError(message, errorCode);
        }

        // File Upload Response Factory Methods
        public static FileUploadResponse CreateFileUploadSuccess(object file, string fileName, long fileSize, string fileType, string fileUrl, string uploadedBy, string message = "File uploaded successfully")
        {
            return FileUploadResponse.CreateSuccess(file, fileName, fileSize, fileType, fileUrl, uploadedBy, message);
        }

        public static FileUploadResponse CreateFileUploadError(string message, string? errorCode = null)
        {
            return FileUploadResponse.CreateError(message, errorCode);
        }

        // Email Response Factory Methods
        public static EmailResponse CreateEmailSuccess(object email, string messageId, string recipient, string subject, string templateName, string message = "Email sent successfully")
        {
            return EmailResponse.CreateSuccess(email, messageId, recipient, subject, templateName, message);
        }

        public static EmailResponse CreateEmailQueued(object email, string messageId, string recipient, string subject, string templateName, string message = "Email queued successfully")
        {
            return EmailResponse.CreateQueued(email, messageId, recipient, subject, templateName, message);
        }

        public static EmailResponse CreateEmailError(string message, string? errorCode = null)
        {
            return EmailResponse.CreateError(message, errorCode);
        }
    }
} 