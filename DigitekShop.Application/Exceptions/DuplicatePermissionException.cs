using System;

namespace DigitekShop.Application.Exceptions
{
    public class DuplicatePermissionException : BadRequestException
    {
        public string PermissionName { get; }

        public DuplicatePermissionException(string permissionName)
            : base($"Permission with name '{permissionName}' already exists")
        {
            PermissionName = permissionName;
        }

        public DuplicatePermissionException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 