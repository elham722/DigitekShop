using System;
using System.Linq;

namespace DigitekShop.Domain.ValueObjects
{
    public class Address
    {
        public string Province { get; private set; }
        public string City { get; private set; }
        public string District { get; private set; }
        public string Street { get; private set; }
        public string Alley { get; private set; }
        public string Building { get; private set; }
        public string Unit { get; private set; }
        public string PostalCode { get; private set; }
        public string Description { get; private set; }

        public Address(string province, string city, string district, string street, 
            string alley = "", string building = "", string unit = "", string postalCode = "", string description = "")
        {
            if (string.IsNullOrWhiteSpace(province))
                throw new ArgumentException("Province cannot be empty");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty");

            if (string.IsNullOrWhiteSpace(district))
                throw new ArgumentException("District cannot be empty");

            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty");

            Province = province.Trim();
            City = city.Trim();
            District = district.Trim();
            Street = street.Trim();
            Alley = alley?.Trim() ?? "";
            Building = building?.Trim() ?? "";
            Unit = unit?.Trim() ?? "";
            PostalCode = postalCode?.Trim() ?? "";
            Description = description?.Trim() ?? "";
        }

        public string GetFullAddress()
        {
            var parts = new[]
            {
                Province,
                City,
                District,
                Street,
                Alley,
                Building,
                Unit
            }.Where(p => !string.IsNullOrWhiteSpace(p));

            return string.Join("، ", parts);
        }

        public string GetShortAddress()
        {
            return $"{City}، {District}";
        }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(Province) &&
                   !string.IsNullOrWhiteSpace(City) &&
                   !string.IsNullOrWhiteSpace(District) &&
                   !string.IsNullOrWhiteSpace(Street);
        }

        public bool HasPostalCode()
        {
            return !string.IsNullOrWhiteSpace(PostalCode);
        }

        public bool IsInTehran()
        {
            return Province.Equals("تهران", StringComparison.OrdinalIgnoreCase);
        }

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
                       Alley == other.Alley &&
                       Building == other.Building &&
                       Unit == other.Unit;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Province, City, District, Street, Alley, Building, Unit);
        }
    }
} 