using System;
using System.Text.RegularExpressions;

namespace DigitekShop.Domain.ValueObjects
{
    public class SKU
    {
        public string Value { get; private set; }

        public SKU(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("SKU cannot be empty");

            if (value.Length < 3)
                throw new ArgumentException("SKU must be at least 3 characters long");

            if (value.Length > 50)
                throw new ArgumentException("SKU cannot exceed 50 characters");

            if (!IsValidSKU(value))
                throw new ArgumentException("SKU contains invalid characters");

            Value = value.Trim().ToUpper();
        }

        private static bool IsValidSKU(string sku)
        {
            // حروف انگلیسی، اعداد، خط تیره و زیرخط
            var pattern = @"^[A-Za-z0-9\-_]+$";
            return Regex.IsMatch(sku, pattern);
        }

        public static SKU Generate(string prefix = "SKU", int length = 8)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            var skuValue = $"{prefix}-{new string(result)}";
            return new SKU(skuValue);
        }

        public string GetPrefix()
        {
            var parts = Value.Split('-');
            return parts.Length > 1 ? parts[0] : "";
        }

        public string GetNumber()
        {
            var parts = Value.Split('-');
            return parts.Length > 1 ? parts[1] : Value;
        }

        public bool HasPrefix(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return false;

            return Value.StartsWith(prefix + "-", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsNumeric()
        {
            return Regex.IsMatch(Value, @"^\d+$");
        }

        public bool IsAlphanumeric()
        {
            return Regex.IsMatch(Value, @"^[A-Za-z0-9]+$");
        }

        public int GetLength() => Value.Length;

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            if (obj is SKU other)
                return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        public override int GetHashCode() => Value.ToUpper().GetHashCode();

        public static implicit operator string(SKU sku) => sku.Value;
    }
} 