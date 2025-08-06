using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل الزامی است")
                .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است")
                .MaximumLength(256).WithMessage("ایمیل نمی‌تواند بیش از 256 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("فرمت ایمیل نامعتبر است");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("توکن بازنشانی الزامی است")
                .MinimumLength(10).WithMessage("توکن بازنشانی باید حداقل 10 کاراکتر باشد")
                .MaximumLength(500).WithMessage("توکن بازنشانی نمی‌تواند بیش از 500 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9\-._~:/?#[\]@!$&'()*+,;=%]*$")
                .WithMessage("توکن بازنشانی شامل کاراکترهای نامعتبر است");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("رمز عبور جدید الزامی است")
                .MinimumLength(8).WithMessage("رمز عبور جدید باید حداقل 8 کاراکتر باشد")
                .MaximumLength(128).WithMessage("رمز عبور جدید نمی‌تواند بیش از 128 کاراکتر باشد")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("رمز عبور جدید باید شامل حداقل یک حرف کوچک، یک حرف بزرگ، یک عدد و یک کاراکتر خاص باشد");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("تایید رمز عبور الزامی است")
                .Equal(x => x.NewPassword).WithMessage("رمز عبور و تایید آن مطابقت ندارند");
        }
    }
} 