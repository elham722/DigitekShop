using System;

namespace DigitekShop.Application.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public string Resource { get; }
        public string Action { get; }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string resource, string action)
            : base($"Access denied to {action} on {resource}")
        {
            Resource = resource;
            Action = action;
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 