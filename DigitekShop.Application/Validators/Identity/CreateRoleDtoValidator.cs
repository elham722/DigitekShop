using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام نقش الزامی است")
                .MinimumLength(2).WithMessage("نام نقش باید حداقل 2 کاراکتر باشد")
                .MaximumLength(50).WithMessage("نام نقش نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\u0600-\u06FF\s]+$").WithMessage("نام نقش فقط می‌تواند شامل حروف، اعداد، _ و فاصله باشد");

            RuleFor(x => x.Description)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("توضیحات نقش نمی‌تواند بیشتر از 200 کاراکتر باشد");

            RuleFor(x => x.Permissions)
                .NotNull().WithMessage("مجوزها نمی‌تواند خالی باشد");

            RuleFor(x => x.CreatedBy)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.CreatedBy))
                .WithMessage("نام ایجادکننده نمی‌تواند بیشتر از 100 کاراکتر باشد");
        }
    }
} 