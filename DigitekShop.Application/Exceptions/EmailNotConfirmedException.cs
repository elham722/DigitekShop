using System;

namespace DigitekShop.Application.Exceptions
{
    public class EmailNotConfirmedException : ApplicationException
    {
        public string Email { get; }
        public bool ResendTokenAvailable { get; }

        public EmailNotConfirmedException(string email, bool resendTokenAvailable = true)
            : base($"Email {email} is not confirmed{(resendTokenAvailable ? ". You can request a new confirmation email" : "")}")
        {
            Email = email;
            ResendTokenAvailable = resendTokenAvailable;
        }

        public EmailNotConfirmedException(string message) : base(message)
        {
        }

        public EmailNotConfirmedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 