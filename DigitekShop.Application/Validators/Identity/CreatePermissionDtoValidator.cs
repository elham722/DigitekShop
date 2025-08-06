using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class CreatePermissionDtoValidator : AbstractValidator<CreatePermissionDto>
    {
        public CreatePermissionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام مجوز الزامی است")
                .MaximumLength(100).WithMessage("نام مجوز نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\.]+$").WithMessage("نام مجوز فقط می‌تواند شامل حروف، اعداد، نقطه و زیرخط باشد");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("توضیحات مجوز الزامی است")
                .MaximumLength(500).WithMessage("توضیحات مجوز نمی‌تواند بیشتر از 500 کاراکتر باشد");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("دسته‌بندی مجوز الزامی است")
                .MaximumLength(50).WithMessage("دسته‌بندی مجوز نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.Resource)
                .NotEmpty().WithMessage("منبع مجوز الزامی است")
                .MaximumLength(100).WithMessage("منبع مجوز نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("عملیات مجوز الزامی است")
                .MaximumLength(50).WithMessage("عملیات مجوز نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .IsInEnum().WithMessage("عملیات مجوز معتبر نیست");
        }
    }
} 