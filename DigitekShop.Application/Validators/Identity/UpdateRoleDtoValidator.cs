using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator()
        {
            // Name validation - optional but if provided, must be valid
            RuleFor(x => x.Name)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("نام نقش نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("نام نقش باید حداقل 2 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\-\s]+$").When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("نام نقش فقط می‌تواند شامل حروف، اعداد، خط تیره، زیرخط و فاصله باشد");

            // Description validation - optional but if provided, must be valid
            RuleFor(x => x.Description)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("توضیحات نقش نمی‌تواند بیشتر از 500 کاراکتر باشد")
                .MinimumLength(10).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("توضیحات نقش باید حداقل 10 کاراکتر باشد");

            // Permissions validation - optional but if provided, must be valid
            RuleFor(x => x.Permissions)
                .Must(permissions => permissions == null || permissions.All(p => !string.IsNullOrEmpty(p)))
                .When(x => x.Permissions != null)
                .WithMessage("لیست مجوزها نمی‌تواند شامل مقادیر خالی باشد");

            RuleForEach(x => x.Permissions)
                .MaximumLength(100).When(x => x.Permissions != null)
                .WithMessage("نام مجوز نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\.]+$").When(x => x.Permissions != null)
                .WithMessage("نام مجوز فقط می‌تواند شامل حروف، اعداد، نقطه و زیرخط باشد");

            // UpdatedBy validation - optional but if provided, must be valid
            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.UpdatedBy))
                .WithMessage("شناسه کاربر بروزرسانی کننده نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\-\.@]+$").When(x => !string.IsNullOrEmpty(x.UpdatedBy))
                .WithMessage("شناسه کاربر بروزرسانی کننده فقط می‌تواند شامل حروف، اعداد، خط تیره، نقطه، زیرخط و @ باشد");

            // Custom validation: At least one field must be provided for update
            RuleFor(x => x)
                .Must(role => !string.IsNullOrEmpty(role.Name) || 
                              !string.IsNullOrEmpty(role.Description) || 
                              role.IsActive.HasValue || 
                              (role.Permissions != null && role.Permissions.Any()))
                .WithMessage("حداقل یکی از فیلدها باید برای بروزرسانی ارائه شود");
        }
    }
} 