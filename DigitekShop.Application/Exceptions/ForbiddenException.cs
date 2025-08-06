using System;

namespace DigitekShop.Application.Exceptions
{
    public class ForbiddenException : ApplicationException
    {
        public string UserId { get; }
        public string RequiredPermission { get; }
        public string Resource { get; }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string userId, string requiredPermission, string resource = null)
            : base($"User {userId} does not have permission '{requiredPermission}'{(resource != null ? $" on {resource}" : "")}")
        {
            UserId = userId;
            RequiredPermission = requiredPermission;
            Resource = resource;
        }

        public ForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 