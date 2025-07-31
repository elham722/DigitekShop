using System;

namespace DigitekShop.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }
        
        public Money(decimal amount, string currency = "IRR")
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative");
                
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty");
                
            Amount = amount;
            Currency = currency.ToUpper();
        }
        
        // Business Operations
        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot add different currencies");
                
            return new Money(Amount + other.Amount, Currency);
        }
        
        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot subtract different currencies");
                
            var result = Amount - other.Amount;
            if (result < 0)
                throw new InvalidOperationException("Result cannot be negative");
                
            return new Money(result, Currency);
        }
        
        public Money ApplyDiscount(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100");
                
            return new Money(Amount * (1 - percentage / 100), Currency);
        }
        
        public Money ApplyTax(decimal taxRate)
        {
            if (taxRate < 0)
                throw new ArgumentException("Tax rate cannot be negative");
                
            return new Money(Amount * (1 + taxRate / 100), Currency);
        }
        
        public bool IsExpensive() => Amount > 1000000; // بیش از 1 میلیون تومان
        
        public string ToDisplayString() => $"{Amount:N0} {Currency}";
        
        public static implicit operator decimal(Money money) => money.Amount;
        
        public static explicit operator Money(decimal amount) => new Money(amount);
        
        public override string ToString() => ToDisplayString();
        
        public override bool Equals(object obj)
        {
            if (obj is Money other)
                return Amount == other.Amount && Currency == other.Currency;
            return false;
        }
        
        public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    }
} 