using System;

namespace DigitekShop.Application.Exceptions
{
    public class InvalidOperationException : ApplicationException
    {
        public string Operation { get; }
        public string EntityName { get; }
        public string Reason { get; }

        public InvalidOperationException(string operation, string entityName, string reason)
            : base($"Cannot perform {operation} on {entityName}: {reason}")
        {
            Operation = operation;
            EntityName = entityName;
            Reason = reason;
        }

        public InvalidOperationException(string message) : base(message)
        {
        }

        public InvalidOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 