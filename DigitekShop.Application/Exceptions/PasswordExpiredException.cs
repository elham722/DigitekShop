using System;

namespace DigitekShop.Application.Exceptions
{
    public class PasswordExpiredException : BadRequestException
    {
        public string UserId { get; }
        public DateTime? ExpiredAt { get; }
        public int DaysSinceExpiry { get; }

        public PasswordExpiredException(string userId, DateTime? expiredAt = null, int daysSinceExpiry = 0)
            : base($"Password for user {userId} has expired{(expiredAt.HasValue ? $" on {expiredAt.Value:yyyy-MM-dd}" : "")}{(daysSinceExpiry > 0 ? $" ({daysSinceExpiry} days ago)" : "")}")
        {
            UserId = userId;
            ExpiredAt = expiredAt;
            DaysSinceExpiry = daysSinceExpiry;
        }

        public PasswordExpiredException(string message) : base(message)
        {
        }

        public PasswordExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 