using System;

namespace DigitekShop.Application.Exceptions
{
    public class DuplicateRoleException : BadRequestException
    {
        public string RoleName { get; }

        public DuplicateRoleException(string roleName)
            : base($"Role with name '{roleName}' already exists")
        {
            RoleName = roleName;
        }

        public DuplicateRoleException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 