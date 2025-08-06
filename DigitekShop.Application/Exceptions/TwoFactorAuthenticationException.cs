using System;

namespace DigitekShop.Application.Exceptions
{
    public class TwoFactorAuthenticationException : BadRequestException
    {
        public string UserId { get; }
        public string Operation { get; }
        public string Reason { get; }

        public TwoFactorAuthenticationException(string userId, string operation, string reason = null)
            : base($"Two-factor authentication {operation} failed for user {userId}{(reason != null ? $". Reason: {reason}" : "")}")
        {
            UserId = userId;
            Operation = operation;
            Reason = reason;
        }

        public TwoFactorAuthenticationException(string message) : base(message)
        {
        }

        public TwoFactorAuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 