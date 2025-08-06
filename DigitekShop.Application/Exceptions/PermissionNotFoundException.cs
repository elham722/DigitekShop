using System;

namespace DigitekShop.Application.Exceptions
{
    public class PermissionNotFoundException : NotFoundException
    {
        public string PermissionId { get; }
        public string PermissionName { get; }

        public PermissionNotFoundException(string permissionId)
            : base($"Permission with ID {permissionId} was not found")
        {
            PermissionId = permissionId;
        }

        public PermissionNotFoundException(string permissionName, bool isName = true)
            : base($"Permission with name '{permissionName}' was not found")
        {
            PermissionName = permissionName;
        }

        public PermissionNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 