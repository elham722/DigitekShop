using System;

namespace DigitekShop.Application.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public string UserId { get; }
        public string Email { get; }

        public UserNotFoundException(string userId)
            : base($"User with ID {userId} was not found")
        {
            UserId = userId;
        }

        public UserNotFoundException(string email, bool isEmail = true)
            : base($"User with email {email} was not found")
        {
            Email = email;
        }

        public UserNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 