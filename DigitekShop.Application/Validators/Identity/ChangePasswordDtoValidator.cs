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
                .MinimumLength(6).WithMessage("رمز عبور فعلی باید حداقل 6 کاراکتر باشد")
                .MaximumLength(128).WithMessage("رمز عبور فعلی نمی‌تواند بیش از 128 کاراکتر باشد")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
                .WithMessage("رمز عبور فعلی باید شامل حداقل یک حرف کوچک، یک حرف بزرگ، یک عدد و یک کاراکتر خاص باشد");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("رمز عبور جدید الزامی است")
                .MinimumLength(8).WithMessage("رمز عبور جدید باید حداقل 8 کاراکتر باشد")
                .MaximumLength(128).WithMessage("رمز عبور جدید نمی‌تواند بیش از 128 کاراکتر باشد")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("رمز عبور جدید باید شامل حداقل یک حرف کوچک، یک حرف بزرگ، یک عدد و یک کاراکتر خاص باشد")
                .NotEqual(x => x.CurrentPassword).WithMessage("رمز عبور جدید نباید با رمز عبور فعلی یکسان باشد");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("تایید رمز عبور الزامی است")
                .Equal(x => x.NewPassword).WithMessage("رمز عبور و تایید آن مطابقت ندارند");
        }
    }
} 