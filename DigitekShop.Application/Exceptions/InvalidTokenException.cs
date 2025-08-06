using System;

namespace DigitekShop.Application.Exceptions
{
    public class InvalidTokenException : BadRequestException
    {
        public string Token { get; }
        public string TokenType { get; }
        public string Reason { get; }

        public InvalidTokenException(string token, string tokenType, string reason = null)
            : base($"Invalid {tokenType} token{(reason != null ? $". Reason: {reason}" : "")}")
        {
            Token = token;
            TokenType = tokenType;
            Reason = reason;
        }

        public InvalidTokenException(string message) : base(message)
        {
        }

        public InvalidTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 