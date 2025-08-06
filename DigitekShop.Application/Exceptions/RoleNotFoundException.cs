using System;

namespace DigitekShop.Application.Exceptions
{
    public class RoleNotFoundException : NotFoundException
    {
        public string RoleId { get; }
        public string RoleName { get; }

        public RoleNotFoundException(string roleId)
            : base($"Role with ID {roleId} was not found")
        {
            RoleId = roleId;
        }

        public RoleNotFoundException(string roleName, bool isName = true)
            : base($"Role with name '{roleName}' was not found")
        {
            RoleName = roleName;
        }

        public RoleNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 