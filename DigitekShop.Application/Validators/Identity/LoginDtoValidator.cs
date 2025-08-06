using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل الزامی است")
                .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است")
                .MaximumLength(256).WithMessage("ایمیل نمی‌تواند بیش از 256 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("فرمت ایمیل نامعتبر است");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز عبور الزامی است")
                .MinimumLength(6).WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد")
                .MaximumLength(128).WithMessage("رمز عبور نمی‌تواند بیش از 128 کاراکتر باشد")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
                .WithMessage("رمز عبور باید شامل حداقل یک حرف بزرگ، یک حرف کوچک، یک عدد و یک کاراکتر خاص باشد");

            RuleFor(x => x.RememberMe)
                .NotNull().WithMessage("فیلد به خاطر سپردن باید مشخص شود");

            RuleFor(x => x.ReturnUrl)
                .MaximumLength(500).WithMessage("آدرس بازگشت نمی‌تواند بیش از 500 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9\-._~:/?#[\]@!$&'()*+,;=%]*$")
                .When(x => !string.IsNullOrEmpty(x.ReturnUrl))
                .WithMessage("آدرس بازگشت شامل کاراکترهای نامعتبر است")
                .Must(url => !url?.Contains("javascript:", StringComparison.OrdinalIgnoreCase) ?? true)
                .When(x => !string.IsNullOrEmpty(x.ReturnUrl))
                .WithMessage("آدرس بازگشت نمی‌تواند شامل کد جاوااسکریپت باشد");
        }
    }
} 