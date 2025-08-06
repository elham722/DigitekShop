using System;

namespace DigitekShop.Application.Exceptions
{
    public class InvalidPasswordException : BadRequestException
    {
        public string UserId { get; }
        public string Reason { get; }

        public InvalidPasswordException(string userId, string reason = null)
            : base($"Invalid password for user {userId}{(reason != null ? $". Reason: {reason}" : "")}")
        {
            UserId = userId;
            Reason = reason;
        }

        public InvalidPasswordException(string message) : base(message)
        {
        }

        public InvalidPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 