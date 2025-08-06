using System;

namespace DigitekShop.Application.Exceptions
{
    public class InvalidCredentialsException : ApplicationException
    {
        public string Email { get; }
        public int RemainingAttempts { get; }

        public InvalidCredentialsException(string email, int remainingAttempts = 0)
            : base($"Invalid credentials for email {email}{(remainingAttempts > 0 ? $". {remainingAttempts} attempts remaining" : "")}")
        {
            Email = email;
            RemainingAttempts = remainingAttempts;
        }

        public InvalidCredentialsException(string message) : base(message)
        {
        }

        public InvalidCredentialsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 