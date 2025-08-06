using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses.Identity
{
    public class RoleResponse : BaseResponse
    {
        [JsonPropertyName("role")]
        public object? Role { get; set; }

        [JsonPropertyName("totalCount")]
        public int? TotalCount { get; set; }

        [JsonPropertyName("pageNumber")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int? PageSize { get; set; }

        [JsonPropertyName("permissions")]
        public IEnumerable<string>? Permissions { get; set; }

        [JsonPropertyName("users")]
        public IEnumerable<string>? Users { get; set; }

        public RoleResponse(bool success, string message = "") : base(success, message)
        {
        }

        public static RoleResponse CreateSuccess(object role, string message = "Role operation successful")
        {
            return new RoleResponse(true, message)
            {
                Role = role
            };
        }

        public static RoleResponse CreatePagedSuccess(object roles, int totalCount, int pageNumber, int pageSize, string message = "Roles retrieved successfully")
        {
            return new RoleResponse(true, message)
            {
                Role = roles,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public static RoleResponse CreateWithPermissions(object role, IEnumerable<string> permissions, string message = "Role with permissions retrieved successfully")
        {
            return new RoleResponse(true, message)
            {
                Role = role,
                Permissions = permissions
            };
        }

        public static RoleResponse CreateWithUsers(object role, IEnumerable<string> users, string message = "Role with users retrieved successfully")
        {
            return new RoleResponse(true, message)
            {
                Role = role,
                Users = users
            };
        }

        public static RoleResponse CreateError(string message, string? errorCode = null)
        {
            return new RoleResponse(false, message);
        }
    }
} 