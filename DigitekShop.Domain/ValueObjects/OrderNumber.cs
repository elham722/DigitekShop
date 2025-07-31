using System;
using System.Text.RegularExpressions;

namespace DigitekShop.Domain.ValueObjects
{
    public class OrderNumber
    {
        public string Value { get; private set; }

        public OrderNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Order number cannot be empty");

            if (!IsValidFormat(value))
                throw new ArgumentException("Invalid order number format");

            Value = value.Trim().ToUpper();
        }

        public static OrderNumber Generate()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            return new OrderNumber($"ORD-{timestamp}-{random}");
        }

        private static bool IsValidFormat(string value)
        {
            // فرمت: ORD-YYYYMMDDHHMMSS-XXXX
            var pattern = @"^ORD-\d{14}-\d{4}$";
            return Regex.IsMatch(value, pattern);
        }

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            if (obj is OrderNumber other)
                return Value == other.Value;
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string(OrderNumber orderNumber) => orderNumber.Value;
    }
} 