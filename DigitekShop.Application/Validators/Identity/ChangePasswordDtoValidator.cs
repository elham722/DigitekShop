using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("رمز عبور فعلی الزامی است")
                .MinimumLength(1).WithMessage("رمز عبور فعلی نمی‌تواند خالی باشد")
                .MaximumLength(100).WithMessage("رمز عبور فعلی نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("رمز عبور جدید الزامی است")
                .MinimumLength(8).WithMessage("رمز عبور جدید باید حداقل 8 کاراکتر باشد")
                .MaximumLength(100).WithMessage("رمز عبور جدید نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("رمز عبور جدید باید شامل حروف کوچک، بزرگ، عدد و کاراکتر خاص باشد")
                .NotEqual(x => x.CurrentPassword).WithMessage("رمز عبور جدید نباید با رمز عبور فعلی یکسان باشد");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Password confirmation does not match");
        }
    }
} 