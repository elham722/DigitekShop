using System;

namespace DigitekShop.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public string EntityName { get; }
        public object Key { get; }

        public NotFoundException(string entityName, object key)
            : base($"{entityName} ({key}) was not found")
        {
            EntityName = entityName;
            Key = key;
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
