using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses.Identity
{
    public class PermissionResponse : BaseResponse
    {
        [JsonPropertyName("permission")]
        public object? Permission { get; set; }

        [JsonPropertyName("totalCount")]
        public int? TotalCount { get; set; }

        [JsonPropertyName("pageNumber")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int? PageSize { get; set; }

        [JsonPropertyName("roles")]
        public IEnumerable<string>? Roles { get; set; }

        [JsonPropertyName("users")]
        public IEnumerable<string>? Users { get; set; }

        public PermissionResponse(bool success, string message = "") : base(success, message)
        {
        }

        public static PermissionResponse CreateSuccess(object permission, string message = "Permission operation successful")
        {
            return new PermissionResponse(true, message)
            {
                Permission = permission
            };
        }

        public static PermissionResponse CreatePagedSuccess(object permissions, int totalCount, int pageNumber, int pageSize, string message = "Permissions retrieved successfully")
        {
            return new PermissionResponse(true, message)
            {
                Permission = permissions,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public static PermissionResponse CreateWithRoles(object permission, IEnumerable<string> roles, string message = "Permission with roles retrieved successfully")
        {
            return new PermissionResponse(true, message)
            {
                Permission = permission,
                Roles = roles
            };
        }

        public static PermissionResponse CreateWithUsers(object permission, IEnumerable<string> users, string message = "Permission with users retrieved successfully")
        {
            return new PermissionResponse(true, message)
            {
                Permission = permission,
                Users = users
            };
        }

        public static PermissionResponse CreateError(string message, string? errorCode = null)
        {
            return new PermissionResponse(false, message);
        }
    }
} 