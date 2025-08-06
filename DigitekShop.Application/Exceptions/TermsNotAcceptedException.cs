using System;

namespace DigitekShop.Application.Exceptions
{
    public class TermsNotAcceptedException : BadRequestException
    {
        public string TermsType { get; }

        public TermsNotAcceptedException(string termsType = "Terms and Conditions")
            : base($"User must accept {termsType} to proceed with registration")
        {
            TermsType = termsType;
        }

        public TermsNotAcceptedException(string message, Exception innerException) : base(message, innerException)
        {
            TermsType = "Terms and Conditions";
        }
    }
} 