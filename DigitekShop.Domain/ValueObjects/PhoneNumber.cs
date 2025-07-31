using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace DigitekShop.Domain.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; private set; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number cannot be empty");

            if (!IsValidPhoneNumber(value))
                throw new ArgumentException("Invalid phone number format");

            Value = NormalizePhoneNumber(value);
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            // فرمت‌های مختلف شماره تلفن ایران
            var patterns = new[]
            {
                @"^09\d{9}$", // موبایل
                @"^0\d{10}$", // ثابت
                @"^\+98\d{10}$", // بین‌المللی
                @"^0098\d{10}$" // بین‌المللی با 00
            };

            return patterns.Any(pattern => Regex.IsMatch(phoneNumber, pattern));
        }

        private static string NormalizePhoneNumber(string phoneNumber)
        {
            // حذف کاراکترهای غیرضروری
            var cleaned = Regex.Replace(phoneNumber, @"[^\d+]", "");

            // تبدیل به فرمت استاندارد
            if (cleaned.StartsWith("+98"))
                return cleaned;
            else if (cleaned.StartsWith("0098"))
                return "+98" + cleaned.Substring(4);
            else if (cleaned.StartsWith("09"))
                return "+98" + cleaned.Substring(1);
            else if (cleaned.StartsWith("0"))
                return "+98" + cleaned.Substring(1);
            else
                return "+98" + cleaned;
        }

        public bool IsMobile()
        {
            return Value.EndsWith("9") && Value.Length == 13;
        }

        public bool IsLandline()
        {
            return !IsMobile();
        }

        public string GetLocalFormat()
        {
            if (Value.StartsWith("+98"))
            {
                var number = Value.Substring(3);
                if (number.StartsWith("9"))
                    return "0" + number;
                else
                    return "0" + number;
            }
            return Value;
        }

        public string GetInternationalFormat()
        {
            return Value;
        }

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            if (obj is PhoneNumber other)
                return Value == other.Value;
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
    }
} 