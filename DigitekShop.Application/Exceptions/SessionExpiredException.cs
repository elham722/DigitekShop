using System;

namespace DigitekShop.Application.Exceptions
{
    public class SessionExpiredException : UnauthorizedException
    {
        public string SessionId { get; }
        public string UserId { get; }
        public DateTime? ExpiredAt { get; }

        public SessionExpiredException(string sessionId, string userId, DateTime? expiredAt = null)
            : base($"Session {sessionId} for user {userId} has expired{(expiredAt.HasValue ? $" at {expiredAt.Value:yyyy-MM-dd HH:mm:ss}" : "")}")
        {
            SessionId = sessionId;
            UserId = userId;
            ExpiredAt = expiredAt;
        }

        public SessionExpiredException(string message) : base(message)
        {
        }

        public SessionExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 