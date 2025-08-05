using System;
using FluentValidation;
using DigitekShop.Application.Features.Customers.Commands.CreateCustomer;

namespace DigitekShop.Application.Validators.Customers
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("نام الزامی است")
                .MaximumLength(50).WithMessage("نام نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("نام فقط می‌تواند شامل حروف فارسی باشد");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("نام خانوادگی الزامی است")
                .MaximumLength(50).WithMessage("نام خانوادگی نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("نام خانوادگی فقط می‌تواند شامل حروف فارسی باشد");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل الزامی است")
                .EmailAddress().WithMessage("فرمت ایمیل معتبر نیست")
                .MaximumLength(100).WithMessage("ایمیل نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("شماره تلفن الزامی است")
                .Matches(@"^09\d{9}$").WithMessage("شماره تلفن باید با 09 شروع شود و 11 رقم باشد");

            RuleFor(x => x.MiddleName)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.MiddleName)).WithMessage("نام میانی نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now.AddYears(-10)).When(x => x.DateOfBirth.HasValue).WithMessage("سن مشتری باید حداقل 10 سال باشد")
                .GreaterThan(DateTime.Now.AddYears(-120)).When(x => x.DateOfBirth.HasValue).WithMessage("تاریخ تولد معتبر نیست");

            RuleFor(x => x.Gender)
                .MaximumLength(10).When(x => !string.IsNullOrEmpty(x.Gender)).WithMessage("جنسیت نمی‌تواند بیشتر از 10 کاراکتر باشد");

            RuleFor(x => x.NationalCode)
                .Length(10).When(x => !string.IsNullOrEmpty(x.NationalCode)).WithMessage("کد ملی باید 10 رقم باشد")
                .Matches(@"^\d{10}$").When(x => !string.IsNullOrEmpty(x.NationalCode)).WithMessage("کد ملی فقط می‌تواند شامل اعداد باشد");

            RuleFor(x => x.PassportNumber)
                .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.PassportNumber)).WithMessage("شماره پاسپورت نمی‌تواند بیشتر از 20 کاراکتر باشد");

            RuleFor(x => x.Address)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Address)).WithMessage("آدرس نمی‌تواند بیشتر از 200 کاراکتر باشد");

            RuleFor(x => x.City)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.City)).WithMessage("نام شهر نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.State)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.State)).WithMessage("نام استان نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.Country)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Country)).WithMessage("نام کشور نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.PostalCode)
                .Matches(@"^\d{10}$").When(x => !string.IsNullOrEmpty(x.PostalCode)).WithMessage("کد پستی باید 10 رقم باشد");

            RuleFor(x => x.Bio)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Bio)).WithMessage("بیوگرافی نمی‌تواند بیشتر از 500 کاراکتر باشد");

            RuleFor(x => x.Website)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Website)).WithMessage("وب‌سایت نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.Website)).WithMessage("آدرس وب‌سایت باید معتبر باشد");

            RuleFor(x => x.Company)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Company)).WithMessage("نام شرکت نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.JobTitle)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.JobTitle)).WithMessage("عنوان شغلی نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Notes)).WithMessage("یادداشت نمی‌تواند بیشتر از 1000 کاراکتر باشد");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
} 