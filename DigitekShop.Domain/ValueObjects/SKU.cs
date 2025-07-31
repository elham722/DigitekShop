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
                
            if (value.Length < 5)
                throw new ArgumentException("SKU must be at least 5 characters");
                
            if (value.Length > 50)
                throw new ArgumentException("SKU cannot exceed 50 characters");
                
            // حذف فاصله و کاراکترهای خاص
            var cleanValue = value.ToUpper().Replace(" ", "-").Replace("_", "-");
            
            // بررسی فرمت: فقط حروف، اعداد و خط تیره
            if (!Regex.IsMatch(cleanValue, @"^[A-Z0-9\-]+$"))
                throw new ArgumentException("SKU can only contain letters, numbers and hyphens");
                
            Value = cleanValue;
        }
        
        // تولید خودکار SKU
        public static SKU Generate(string category, string brand, string model, string variant = "")
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be empty");
                
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be empty");
                
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty");
                
            var sku = $"{category.ToUpper()}-{brand.ToUpper()}-{model.ToUpper()}";
            
            if (!string.IsNullOrWhiteSpace(variant))
                sku += $"-{variant.ToUpper()}";
                
            return new SKU(sku);
        }
        
        // تولید SKU با شماره سریال
        public static SKU GenerateWithSerial(string prefix, int serialNumber)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("Prefix cannot be empty");
                
            if (serialNumber < 1)
                throw new ArgumentException("Serial number must be positive");
                
            return new SKU($"{prefix.ToUpper()}-{serialNumber:D6}");
        }
        
        public static implicit operator string(SKU sku) => sku.Value;
        
        public static explicit operator SKU(string value) => new SKU(value);
        
        public override string ToString() => Value;
        
        public override bool Equals(object obj)
        {
            if (obj is SKU other)
                return Value.Equals(other.Value);
            return false;
        }
        
        public override int GetHashCode() => Value.GetHashCode();
    }
} 