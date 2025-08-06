using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل الزامی است")
                .EmailAddress().WithMessage("فرمت ایمیل معتبر نیست")
                .MaximumLength(100).WithMessage("ایمیل نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("نام کاربری الزامی است")
                .MinimumLength(3).WithMessage("نام کاربری باید حداقل 3 کاراکتر باشد")
                .MaximumLength(50).WithMessage("نام کاربری نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("نام کاربری فقط می‌تواند شامل حروف انگلیسی، اعداد و _ باشد");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز عبور الزامی است")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل 8 کاراکتر باشد")
                .MaximumLength(100).WithMessage("رمز عبور نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("رمز عبور باید شامل حروف کوچک، بزرگ، عدد و کاراکتر خاص باشد");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("تکرار رمز عبور مطابقت ندارد");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("نام الزامی است")
                .MaximumLength(50).WithMessage("نام نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("نام فقط می‌تواند شامل حروف فارسی باشد");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("نام خانوادگی الزامی است")
                .MaximumLength(50).WithMessage("نام خانوادگی نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("نام خانوادگی فقط می‌تواند شامل حروف فارسی باشد");

            RuleFor(x => x.MiddleName)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.MiddleName))
                .WithMessage("نام میانی نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[\u0600-\u06FF\s]+$").When(x => !string.IsNullOrEmpty(x.MiddleName))
                .WithMessage("نام میانی فقط می‌تواند شامل حروف فارسی باشد");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^09\d{9}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("شماره تلفن باید با 09 شروع شود و 11 رقم باشد");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now.AddYears(-13)).When(x => x.DateOfBirth.HasValue)
                .WithMessage("سن کاربر باید حداقل 13 سال باشد")
                .GreaterThan(DateTime.Now.AddYears(-120)).When(x => x.DateOfBirth.HasValue)
                .WithMessage("تاریخ تولد معتبر نیست");

            RuleFor(x => x.Gender)
                .MaximumLength(10).When(x => !string.IsNullOrEmpty(x.Gender))
                .WithMessage("جنسیت نمی‌تواند بیشتر از 10 کاراکتر باشد")
                .Must(BeValidGender).When(x => !string.IsNullOrEmpty(x.Gender))
                .WithMessage("جنسیت باید یکی از مقادیر 'مرد'، 'زن' یا 'سایر' باشد");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).When(x => x.CustomerId.HasValue)
                .WithMessage("شناسه مشتری باید عدد مثبت باشد");

            RuleFor(x => x.AcceptTerms)
                .Equal(true).WithMessage("پذیرش قوانین و مقررات الزامی است");

            RuleFor(x => x.AcceptTerms)
                .Equal(true).WithMessage("پذیرش قوانین و مقررات الزامی است");

            RuleFor(x => x.AcceptMarketing)
                .NotNull().WithMessage("انتخاب دریافت ایمیل‌های تبلیغاتی الزامی است");

            RuleFor(x => x.DefaultRole)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.DefaultRole))
                .WithMessage("نقش پیش‌فرض نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Must(BeValidRole).When(x => !string.IsNullOrEmpty(x.DefaultRole))
                .WithMessage("نقش پیش‌فرض باید یکی از نقش‌های معتبر باشد");
        }

        private bool BeValidGender(string gender)
        {
            return new[] { "مرد", "زن", "سایر" }.Contains(gender);
        }

        private bool BeValidRole(string role)
        {
            return new[] { "Customer", "User", "Moderator" }.Contains(role);
        }
    }
} 