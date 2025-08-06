using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using DigitekShop.Application.Responses.Identity;
using DigitekShop.Application.Responses.Analytics;
using DigitekShop.Application.Responses.File;
using DigitekShop.Application.Responses.Email;

namespace DigitekShop.Application.Responses
{
    public static class ResponseWrapper
    {
        public static T Wrap<T>(T response, HttpContext? httpContext = null) where T : BaseResponse
        {
            if (httpContext != null)
            {
                // Add request ID if available
                if (httpContext.TraceIdentifier != null)
                {
                    response.RequestId = httpContext.TraceIdentifier;
                }

                // Add correlation ID if available in headers
                if (httpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
                {
                    response.RequestId = correlationId.ToString();
                }
            }

            return response;
        }

        public static SuccessResponse<T> WrapSuccess<T>(T data, string message = "Operation completed successfully", HttpContext? httpContext = null) where T : class
        {
            var response = ResponseFactory.CreateSuccess(data, message);
            return Wrap(response, httpContext);
        }

        public static SuccessResponse<T> WrapPagedSuccess<T>(T data, int totalCount, int pageNumber, int pageSize, string message = "Operation completed successfully", HttpContext? httpContext = null) where T : class
        {
            var response = ResponseFactory.CreatePagedSuccess(data, totalCount, pageNumber, pageSize, message);
            return Wrap(response, httpContext);
        }

        public static CommandResponse<T> WrapCommand<T>(string operation, string message = "Command executed successfully", HttpContext? httpContext = null) where T : class
        {
            var response = ResponseFactory.CreateCommand<T>(operation, message);
            return Wrap(response, httpContext);
        }

        public static CommandResponse<T> WrapCommandWithData<T>(T data, string operation, string message = "Command executed successfully", HttpContext? httpContext = null) where T : class
        {
            var response = ResponseFactory.CreateCommandWithData(data, operation, message);
            return Wrap(response, httpContext);
        }

        public static CommandResponse<T> WrapCommandWithId<T>(int id, string operation, string message = "Command executed successfully", HttpContext? httpContext = null) where T : class
        {
            var response = ResponseFactory.CreateCommandWithId<T>(id, operation, message);
            return Wrap(response, httpContext);
        }

        public static CommandResponse<T> WrapCommandWithDataAndId<T>(T data, int id, string operation, string message = "Command executed successfully", HttpContext? httpContext = null) where T : class
        {
            var response = ResponseFactory.CreateCommandWithDataAndId(data, id, operation, message);
            return Wrap(response, httpContext);
        }

        // Identity Response Wrapper Methods
        public static AuthResponse WrapAuthSuccess(string token, DateTime expiresAt, object user, string message = "Authentication successful", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateAuthSuccess(token, expiresAt, user, message);
            return Wrap(response, httpContext);
        }

        public static AuthResponse WrapAuthTwoFactorRequired(string twoFactorToken, string message = "Two-factor authentication required", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateAuthTwoFactorRequired(twoFactorToken, message);
            return Wrap(response, httpContext);
        }

        public static AuthResponse WrapAuthError(string message, string? errorCode = null, HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateAuthError(message, errorCode);
            return Wrap(response, httpContext);
        }

        public static UserResponse WrapUserSuccess(object user, string message = "User operation successful", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateUserSuccess(user, message);
            return Wrap(response, httpContext);
        }

        public static UserResponse WrapUserPagedSuccess(object users, int totalCount, int pageNumber, int pageSize, string message = "Users retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateUserPagedSuccess(users, totalCount, pageNumber, pageSize, message);
            return Wrap(response, httpContext);
        }

        public static UserResponse WrapUserWithRoles(object user, IEnumerable<string> roles, string message = "User with roles retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateUserWithRoles(user, roles, message);
            return Wrap(response, httpContext);
        }

        public static UserResponse WrapUserWithPermissions(object user, IEnumerable<string> permissions, string message = "User with permissions retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateUserWithPermissions(user, permissions, message);
            return Wrap(response, httpContext);
        }

        public static UserResponse WrapUserError(string message, string? errorCode = null, HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateUserError(message, errorCode);
            return Wrap(response, httpContext);
        }

        public static RoleResponse WrapRoleSuccess(object role, string message = "Role operation successful", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateRoleSuccess(role, message);
            return Wrap(response, httpContext);
        }

        public static RoleResponse WrapRolePagedSuccess(object roles, int totalCount, int pageNumber, int pageSize, string message = "Roles retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateRolePagedSuccess(roles, totalCount, pageNumber, pageSize, message);
            return Wrap(response, httpContext);
        }

        public static RoleResponse WrapRoleWithPermissions(object role, IEnumerable<string> permissions, string message = "Role with permissions retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateRoleWithPermissions(role, permissions, message);
            return Wrap(response, httpContext);
        }

        public static RoleResponse WrapRoleWithUsers(object role, IEnumerable<string> users, string message = "Role with users retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateRoleWithUsers(role, users, message);
            return Wrap(response, httpContext);
        }

        public static RoleResponse WrapRoleError(string message, string? errorCode = null, HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateRoleError(message, errorCode);
            return Wrap(response, httpContext);
        }

        public static PermissionResponse WrapPermissionSuccess(object permission, string message = "Permission operation successful", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreatePermissionSuccess(permission, message);
            return Wrap(response, httpContext);
        }

        public static PermissionResponse WrapPermissionPagedSuccess(object permissions, int totalCount, int pageNumber, int pageSize, string message = "Permissions retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreatePermissionPagedSuccess(permissions, totalCount, pageNumber, pageSize, message);
            return Wrap(response, httpContext);
        }

        public static PermissionResponse WrapPermissionWithRoles(object permission, IEnumerable<string> roles, string message = "Permission with roles retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreatePermissionWithRoles(permission, roles, message);
            return Wrap(response, httpContext);
        }

        public static PermissionResponse WrapPermissionWithUsers(object permission, IEnumerable<string> users, string message = "Permission with users retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreatePermissionWithUsers(permission, users, message);
            return Wrap(response, httpContext);
        }

        public static PermissionResponse WrapPermissionError(string message, string? errorCode = null, HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreatePermissionError(message, errorCode);
            return Wrap(response, httpContext);
        }

        // Analytics Response Wrapper Methods
        public static AnalyticsResponse WrapAnalyticsSuccess(object analytics, string period, DateTime startDate, DateTime endDate, string message = "Analytics retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateAnalyticsSuccess(analytics, period, startDate, endDate, message);
            return Wrap(response, httpContext);
        }

        public static AnalyticsResponse WrapAnalyticsWithMetrics(object analytics, Dictionary<string, object> metrics, string message = "Analytics with metrics retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateAnalyticsWithMetrics(analytics, metrics, message);
            return Wrap(response, httpContext);
        }

        public static AnalyticsResponse WrapAnalyticsWithTrends(object analytics, object trends, string message = "Analytics with trends retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateAnalyticsWithTrends(analytics, trends, message);
            return Wrap(response, httpContext);
        }

        public static AnalyticsResponse WrapAnalyticsError(string message, string? errorCode = null, HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateAnalyticsError(message, errorCode);
            return Wrap(response, httpContext);
        }

        public static ActivityResponse WrapActivitySuccess(object activities, string message = "Activities retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateActivitySuccess(activities, message);
            return Wrap(response, httpContext);
        }

        public static ActivityResponse WrapActivityPagedSuccess(object activities, int totalCount, int pageNumber, int pageSize, string message = "Activities retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateActivityPagedSuccess(activities, totalCount, pageNumber, pageSize, message);
            return Wrap(response, httpContext);
        }

        public static ActivityResponse WrapActivityFiltered(object activities, string activityType, string userId, DateTime startDate, DateTime endDate, string message = "Filtered activities retrieved successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateActivityFiltered(activities, activityType, userId, startDate, endDate, message);
            return Wrap(response, httpContext);
        }

        public static ActivityResponse WrapActivityError(string message, string? errorCode = null, HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateActivityError(message, errorCode);
            return Wrap(response, httpContext);
        }

        // File Upload Response Wrapper Methods
        public static FileUploadResponse WrapFileUploadSuccess(object file, string fileName, long fileSize, string fileType, string fileUrl, string uploadedBy, string message = "File uploaded successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateFileUploadSuccess(file, fileName, fileSize, fileType, fileUrl, uploadedBy, message);
            return Wrap(response, httpContext);
        }

        public static FileUploadResponse WrapFileUploadError(string message, string? errorCode = null, HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateFileUploadError(message, errorCode);
            return Wrap(response, httpContext);
        }

        // Email Response Wrapper Methods
        public static EmailResponse WrapEmailSuccess(object email, string messageId, string recipient, string subject, string templateName, string message = "Email sent successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateEmailSuccess(email, messageId, recipient, subject, templateName, message);
            return Wrap(response, httpContext);
        }

        public static EmailResponse WrapEmailQueued(object email, string messageId, string recipient, string subject, string templateName, string message = "Email queued successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateEmailQueued(email, messageId, recipient, subject, templateName, message);
            return Wrap(response, httpContext);
        }

        public static EmailResponse WrapEmailError(string message, string? errorCode = null, HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateEmailError(message, errorCode);
            return Wrap(response, httpContext);
        }
    }
} 