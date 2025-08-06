using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses.Identity
{
    public class UserResponse : BaseResponse
    {
        [JsonPropertyName("user")]
        public object? User { get; set; }

        [JsonPropertyName("totalCount")]
        public int? TotalCount { get; set; }

        [JsonPropertyName("pageNumber")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int? PageSize { get; set; }

        [JsonPropertyName("roles")]
        public IEnumerable<string>? Roles { get; set; }

        [JsonPropertyName("permissions")]
        public IEnumerable<string>? Permissions { get; set; }

        public UserResponse(bool success, string message = "") : base(success, message)
        {
        }

        public static UserResponse CreateSuccess(object user, string message = "User operation successful")
        {
            return new UserResponse(true, message)
            {
                User = user
            };
        }

        public static UserResponse CreatePagedSuccess(object users, int totalCount, int pageNumber, int pageSize, string message = "Users retrieved successfully")
        {
            return new UserResponse(true, message)
            {
                User = users,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public static UserResponse CreateWithRoles(object user, IEnumerable<string> roles, string message = "User with roles retrieved successfully")
        {
            return new UserResponse(true, message)
            {
                User = user,
                Roles = roles
            };
        }

        public static UserResponse CreateWithPermissions(object user, IEnumerable<string> permissions, string message = "User with permissions retrieved successfully")
        {
            return new UserResponse(true, message)
            {
                User = user,
                Permissions = permissions
            };
        }

        public static UserResponse CreateError(string message, string? errorCode = null)
        {
            return new UserResponse(false, message);
        }
    }
} 