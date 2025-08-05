using System;
using System.Linq;

namespace DigitekShop.Domain.ValueObjects
{
    public class Rating
    {
        public decimal Value { get; private set; }

        public Rating(decimal value)
        {
            if (value < 0 || value > 5)
                throw new ArgumentException("Rating must be between 0 and 5");

            Value = Math.Round(value, 1);
        }

        public static Rating FromInt(int rating)
        {
            if (rating < 0 || rating > 5)
                throw new ArgumentException("Rating must be between 0 and 5");

            return new Rating(rating);
        }

        public static Rating FromDouble(double rating)
        {
            if (rating < 0 || rating > 5)
                throw new ArgumentException("Rating must be between 0 and 5");

            return new Rating((decimal)rating);
        }

        public static Rating CalculateAverage(IEnumerable<int> ratings)
        {
            if (ratings == null || !ratings.Any())
                return new Rating(0);

            var average = ratings.Average(r => (double)r);
            return new Rating((decimal)average);
        }

        public static Rating CalculateAverage(IEnumerable<decimal> ratings)
        {
            if (ratings == null || !ratings.Any())
                return new Rating(0);

            var average = ratings.Average();
            return new Rating(average);
        }

        public bool IsExcellent() => Value >= 4.5m;
        public bool IsGood() => Value >= 4.0m;
        public bool IsAverage() => Value >= 3.0m;
        public bool IsPoor() => Value >= 2.0m;
        public bool IsVeryPoor() => Value < 2.0m;

        public string GetRatingText()
        {
            return Value switch
            {
                >= 4.5m => "عالی",
                >= 4.0m => "خوب",
                >= 3.5m => "بالاتر از متوسط",
                >= 3.0m => "متوسط",
                >= 2.5m => "پایین‌تر از متوسط",
                >= 2.0m => "بد",
                >= 1.0m => "خیلی بد",
                _ => "بدون امتیاز"
            };
        }

        public string GetStars()
        {
            var fullStars = (int)Value;
            var hasHalfStar = Value % 1 >= 0.5m;
            
            var stars = new string('★', fullStars);
            if (hasHalfStar)
                stars += "☆";
            
            return stars.PadRight(5, '☆');
        }

        public override string ToString() => Value.ToString("F1");

        public override bool Equals(object obj)
        {
            if (obj is Rating other)
                return Value == other.Value;
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator decimal(Rating rating) => rating.Value;
        public static implicit operator double(Rating rating) => (double)rating.Value;
        public static implicit operator Rating(decimal value) => new Rating(value);
        public static implicit operator Rating(double value) => new Rating((decimal)value);

        public static bool operator >=(Rating left, Rating right) => left.Value >= right.Value;
        public static bool operator <=(Rating left, Rating right) => left.Value <= right.Value;
        public static bool operator >(Rating left, Rating right) => left.Value > right.Value;
        public static bool operator <(Rating left, Rating right) => left.Value < right.Value;
        public static bool operator ==(Rating left, Rating right) => left.Value == right.Value;
        public static bool operator !=(Rating left, Rating right) => left.Value != right.Value;
    }
} 