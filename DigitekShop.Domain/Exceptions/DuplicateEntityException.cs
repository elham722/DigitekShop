using System;

namespace DigitekShop.Domain.Exceptions
{
    public class DuplicateEntityException : DomainException
    {
        public string EntityType { get; }
        public string PropertyName { get; }
        public object PropertyValue { get; }

        public DuplicateEntityException(string entityType, string propertyName, object propertyValue)
            : base($"{entityType} with {propertyName} '{propertyValue}' already exists.")
        {
            EntityType = entityType;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public DuplicateEntityException(string message) : base(message)
        {
        }
    }
} 