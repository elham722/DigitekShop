using System;

namespace DigitekShop.Application.Exceptions
{
    public class AccountLockedException : ApplicationException
    {
        public string UserId { get; }
        public DateTime? LockoutEnd { get; }
        public string Reason { get; }

        public AccountLockedException(string userId, DateTime? lockoutEnd = null, string reason = null)
            : base($"Account for user {userId} is locked{(lockoutEnd.HasValue ? $" until {lockoutEnd.Value:yyyy-MM-dd HH:mm:ss}" : "")}{(reason != null ? $". Reason: {reason}" : "")}")
        {
            UserId = userId;
            LockoutEnd = lockoutEnd;
            Reason = reason;
        }

        public AccountLockedException(string message) : base(message)
        {
        }

        public AccountLockedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 