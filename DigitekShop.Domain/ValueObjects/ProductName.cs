using System;
using System.Text.RegularExpressions;

namespace DigitekShop.Domain.ValueObjects
{
    public class ProductName
    {
        public string Value { get; private set; }

        public ProductName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name cannot be empty");

            if (value.Length < 3)
                throw new ArgumentException("Product name must be at least 3 characters long");

            if (value.Length > 200)
                throw new ArgumentException("Product name cannot exceed 200 characters");

            if (!IsValidProductName(value))
                throw new ArgumentException("Product name contains invalid characters");

            Value = value.Trim();
        }

        private static bool IsValidProductName(string name)
        {
            // حروف فارسی، انگلیسی، اعداد، فاصله و کاراکترهای خاص مجاز
            var pattern = @"^[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\uFB50-\uFDFF\uFE70-\uFEFFa-zA-Z0-9\s\-_\.\(\)\[\]]+$";
            return Regex.IsMatch(name, pattern);
        }

        public string GetShortName(int maxLength = 50)
        {
            if (Value.Length <= maxLength)
                return Value;

            return Value.Substring(0, maxLength - 3) + "...";
        }

        public bool ContainsKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return false;

            return Value.Contains(keyword, StringComparison.OrdinalIgnoreCase);
        }

        public bool StartsWith(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return false;

            return Value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
        }

        public bool EndsWith(string suffix)
        {
            if (string.IsNullOrWhiteSpace(suffix))
                return false;

            return Value.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
        }

        public int GetWordCount()
        {
            return Value.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public bool IsLongName() => Value.Length > 100;

        public bool IsShortName() => Value.Length < 20;

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            if (obj is ProductName other)
                return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        public override int GetHashCode() => Value.ToLower().GetHashCode();

        public static implicit operator string(ProductName productName) => productName.Value;
    }
} 