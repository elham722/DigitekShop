using System;
using System.Text.RegularExpressions;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.BusinessRules
{
    public class CustomerMustBeActiveRule : BaseBusinessRule
    {
        private readonly Customer _customer;

        public CustomerMustBeActiveRule(Customer customer)
        {
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public override bool IsBroken() => !_customer.IsActive();

        public override string Message => "مشتری باید فعال باشد";

        public override string RuleName => "CustomerMustBeActive";

        public override string ErrorCode => "CUSTOMER_001";

        public override bool IsCritical => true;
    }

    public class CustomerMustNotBeBlockedRule : BaseBusinessRule
    {
        private readonly Customer _customer;

        public CustomerMustNotBeBlockedRule(Customer customer)
        {
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public override bool IsBroken() => _customer.IsBlocked();

        public override string Message => "مشتری مسدود شده نمی‌تواند عملیات انجام دهد";

        public override string RuleName => "CustomerMustNotBeBlocked";

        public override string ErrorCode => "CUSTOMER_002";

        public override bool IsCritical => true;
    }

    public class CustomerEmailMustBeValidRule : BaseBusinessRule
    {
        private readonly Email _email;

        public CustomerEmailMustBeValidRule(Email email)
        {
            _email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public override bool IsBroken() => !IsValidEmail(_email.Value);

        public override string Message => "ایمیل مشتری باید معتبر باشد";

        public override string RuleName => "CustomerEmailMustBeValid";

        public override string ErrorCode => "CUSTOMER_003";

        public override bool IsCritical => true;

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }

    public class CustomerPhoneMustBeValidRule : BaseBusinessRule
    {
        private readonly PhoneNumber _phone;

        public CustomerPhoneMustBeValidRule(PhoneNumber phone)
        {
            _phone = phone ?? throw new ArgumentNullException(nameof(phone));
        }

        public override bool IsBroken() => !IsValidPhone(_phone.Value);

        public override string Message => "شماره تلفن مشتری باید معتبر باشد";

        public override string RuleName => "CustomerPhoneMustBeValid";

        public override string ErrorCode => "CUSTOMER_004";

        public override bool IsCritical => true;

        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // حذف کاراکترهای غیر عددی
            var digitsOnly = Regex.Replace(phone, @"[^\d]", "");
            
            // بررسی طول شماره (حداقل 10 رقم)
            return digitsOnly.Length >= 10;
        }
    }

    public class CustomerNationalCodeMustBeValidRule : BaseBusinessRule
    {
        private readonly string _nationalCode;

        public CustomerNationalCodeMustBeValidRule(string nationalCode)
        {
            _nationalCode = nationalCode;
        }

        public override bool IsBroken() => !IsValidNationalCode(_nationalCode);

        public override string Message => "کد ملی مشتری باید معتبر باشد";

        public override string RuleName => "CustomerNationalCodeMustBeValid";

        public override string ErrorCode => "CUSTOMER_005";

        public override bool IsCritical => false;

        private bool IsValidNationalCode(string nationalCode)
        {
            if (string.IsNullOrWhiteSpace(nationalCode))
                return true; // اختیاری است

            // حذف کاراکترهای غیر عددی
            var digitsOnly = Regex.Replace(nationalCode, @"[^\d]", "");
            
            // بررسی طول کد ملی (10 رقم)
            if (digitsOnly.Length != 10)
                return false;

            // الگوریتم اعتبارسنجی کد ملی ایران
            var sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(digitsOnly[i].ToString()) * (10 - i);
            }

            var remainder = sum % 11;
            var controlDigit = int.Parse(digitsOnly[9].ToString());

            if (remainder < 2)
                return controlDigit == remainder;
            else
                return controlDigit == (11 - remainder);
        }
    }

    public class CustomerAgeMustBeValidRule : BaseBusinessRule
    {
        private readonly DateTime? _dateOfBirth;
        private readonly int _minimumAge;

        public CustomerAgeMustBeValidRule(DateTime? dateOfBirth, int minimumAge = 18)
        {
            _dateOfBirth = dateOfBirth;
            _minimumAge = minimumAge;
        }

        public override bool IsBroken() => !IsValidAge(_dateOfBirth, _minimumAge);

        public override string Message => $"سن مشتری باید حداقل {_minimumAge} سال باشد";

        public override string RuleName => "CustomerAgeMustBeValid";

        public override string ErrorCode => "CUSTOMER_006";

        public override bool IsCritical => false;

        private bool IsValidAge(DateTime? dateOfBirth, int minimumAge)
        {
            if (!dateOfBirth.HasValue)
                return true; // اختیاری است

            var age = DateTime.Today.Year - dateOfBirth.Value.Year;
            if (dateOfBirth.Value.Date > DateTime.Today.AddYears(-age))
                age--;

            return age >= minimumAge;
        }
    }

    public class CustomerNameMustNotBeEmptyRule : BaseBusinessRule
    {
        private readonly string _firstName;
        private readonly string _lastName;

        public CustomerNameMustNotBeEmptyRule(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }

        public override bool IsBroken() => string.IsNullOrWhiteSpace(_firstName) || string.IsNullOrWhiteSpace(_lastName);

        public override string Message => "نام و نام خانوادگی مشتری نمی‌تواند خالی باشد";

        public override string RuleName => "CustomerNameMustNotBeEmpty";

        public override string ErrorCode => "CUSTOMER_007";

        public override bool IsCritical => true;
    }
} 