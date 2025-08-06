using System;

namespace DigitekShop.Application.Exceptions
{
    public class DuplicateUserException : BadRequestException
    {
        public string Email { get; }
        public string UserName { get; }

        public DuplicateUserException(string email)
            : base($"User with email '{email}' already exists")
        {
            Email = email;
        }

        public DuplicateUserException(string userName, bool isUserName = true)
            : base($"User with username '{userName}' already exists")
        {
            UserName = userName;
        }

        public DuplicateUserException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 