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

            if (!IsValidOrderNumber(value))
                throw new ArgumentException("Invalid order number format");

            Value = value.Trim().ToUpper();
        }

        private static bool IsValidOrderNumber(string orderNumber)
        {
            // فرمت: ORD-YYYYMMDD-XXXXX (YYYYMMDD تاریخ، XXXXX شماره سریال)
            var pattern = @"^ORD-\d{8}-\d{5}$";
            return Regex.IsMatch(orderNumber, pattern);
        }

        public static OrderNumber Generate()
        {
            var date = DateTime.UtcNow;
            var datePart = date.ToString("yyyyMMdd");
            var random = new Random();
            var serialPart = random.Next(10000, 99999).ToString("D5");
            
            var orderNumber = $"ORD-{datePart}-{serialPart}";
            return new OrderNumber(orderNumber);
        }

        public static OrderNumber GenerateWithDate(DateTime date)
        {
            var datePart = date.ToString("yyyyMMdd");
            var random = new Random();
            var serialPart = random.Next(10000, 99999).ToString("D5");
            
            var orderNumber = $"ORD-{datePart}-{serialPart}";
            return new OrderNumber(orderNumber);
        }

        public DateTime GetOrderDate()
        {
            var parts = Value.Split('-');
            if (parts.Length >= 2)
            {
                var datePart = parts[1];
                if (DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var date))
                {
                    return date;
                }
            }
            throw new InvalidOperationException("Cannot extract date from order number");
        }

        public string GetSerialNumber()
        {
            var parts = Value.Split('-');
            return parts.Length >= 3 ? parts[2] : "";
        }

        public bool IsFromToday()
        {
            try
            {
                var orderDate = GetOrderDate();
                return orderDate.Date == DateTime.UtcNow.Date;
            }
            catch
            {
                return false;
            }
        }

        public bool IsFromDate(DateTime date)
        {
            try
            {
                var orderDate = GetOrderDate();
                return orderDate.Date == date.Date;
            }
            catch
            {
                return false;
            }
        }

        public bool IsRecent(int days = 7)
        {
            try
            {
                var orderDate = GetOrderDate();
                return orderDate >= DateTime.UtcNow.AddDays(-days);
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            if (obj is OrderNumber other)
                return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        public override int GetHashCode() => Value.ToUpper().GetHashCode();

        public static implicit operator string(OrderNumber orderNumber) => orderNumber.Value;
    }
} 