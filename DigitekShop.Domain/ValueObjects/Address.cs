using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DigitekShop.Domain.ValueObjects
{
    public class Address
    {
        public string Province { get; private set; }
        public string City { get; private set; }
        public string District { get; private set; }
        public string Street { get; private set; }
        public string PostalCode { get; private set; }
        public string Details { get; private set; }

        public Address(string province, string city, string district = "", string street = "", 
            string postalCode = "", string details = "")
        {
            if (string.IsNullOrWhiteSpace(province))
                throw new ArgumentException("Province cannot be empty");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty");

            Province = province.Trim();
            City = city.Trim();
            District = district?.Trim() ?? "";
            Street = street?.Trim() ?? "";
            PostalCode = postalCode?.Trim() ?? "";
            Details = details?.Trim() ?? "";

            if (!string.IsNullOrWhiteSpace(PostalCode) && !IsValidPostalCode(PostalCode))
                throw new ArgumentException("Invalid postal code format");
        }

        private static bool IsValidPostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return true;

            // فرمت کد پستی ایران: 10 رقم
            var pattern = @"^\d{10}$";
            return Regex.IsMatch(postalCode, pattern);
        }

        public string GetFullAddress()
        {
            var parts = new[]
            {
                Province,
                City,
                District,
                Street,
                Details
            };

            return string.Join("، ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        public string GetShortAddress()
        {
            return $"{City}، {Province}";
        }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(Province) && 
                   !string.IsNullOrWhiteSpace(City) && 
                   !string.IsNullOrWhiteSpace(Street);
        }

        public bool HasPostalCode() => !string.IsNullOrWhiteSpace(PostalCode);

        public bool IsInTehran() => Province.Equals("تهران", StringComparison.OrdinalIgnoreCase);

        public bool IsInMajorCity()
        {
            var majorCities = new[] { "تهران", "اصفهان", "مشهد", "شیراز", "تبریز", "اهواز", "کرج" };
            return majorCities.Contains(City);
        }

        public override string ToString() => GetFullAddress();

        public override bool Equals(object obj)
        {
            if (obj is Address other)
            {
                return Province == other.Province &&
                       City == other.City &&
                       District == other.District &&
                       Street == other.Street &&
                       PostalCode == other.PostalCode;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Province, City, District, Street, PostalCode);
        }
    }
} 