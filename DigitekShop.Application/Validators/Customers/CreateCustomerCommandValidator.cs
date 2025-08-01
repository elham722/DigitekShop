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

            RuleFor(x => x.NationalCode)
                .NotEmpty().WithMessage("کد ملی الزامی است")
                .Length(10).WithMessage("کد ملی باید 10 رقم باشد")
                .Matches(@"^\d{10}$").WithMessage("کد ملی فقط می‌تواند شامل اعداد باشد");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("تاریخ تولد الزامی است")
                .LessThan(DateTime.Now.AddYears(-10)).WithMessage("سن مشتری باید حداقل 10 سال باشد")
                .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("تاریخ تولد معتبر نیست");

            RuleFor(x => x.Street)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Street)).WithMessage("آدرس خیابان نمی‌تواند بیشتر از 200 کاراکتر باشد");

            RuleFor(x => x.City)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.City)).WithMessage("نام شهر نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.State)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.State)).WithMessage("نام استان نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.PostalCode)
                .Matches(@"^\d{10}$").When(x => !string.IsNullOrEmpty(x.PostalCode)).WithMessage("کد پستی باید 10 رقم باشد");

            RuleFor(x => x.Country)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Country)).WithMessage("نام کشور نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.ProfileImageUrl)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.ProfileImageUrl)).WithMessage("آدرس تصویر پروفایل نمی‌تواند بیشتر از 500 کاراکتر باشد")
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.ProfileImageUrl)).WithMessage("آدرس تصویر پروفایل باید معتبر باشد");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Notes)).WithMessage("یادداشت نمی‌تواند بیشتر از 1000 کاراکتر باشد");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
} 