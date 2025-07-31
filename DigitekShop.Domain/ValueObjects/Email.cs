using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace DigitekShop.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty");

            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format");

            Value = value.Trim().ToLower();
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        public string GetDomain()
        {
            var parts = Value.Split('@');
            return parts.Length == 2 ? parts[1] : string.Empty;
        }

        public string GetUsername()
        {
            var parts = Value.Split('@');
            return parts.Length == 2 ? parts[0] : string.Empty;
        }

        public bool IsBusinessEmail()
        {
            var domain = GetDomain();
            var businessDomains = new[] { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" };
            return !businessDomains.Contains(domain);
        }

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            if (obj is Email other)
                return Value == other.Value;
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string(Email email) => email.Value;
    }
} 