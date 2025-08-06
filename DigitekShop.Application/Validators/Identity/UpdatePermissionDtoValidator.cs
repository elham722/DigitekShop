using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class UpdatePermissionDtoValidator : AbstractValidator<UpdatePermissionDto>
    {
        public UpdatePermissionDtoValidator()
        {
            // Name validation - optional but if provided, must be valid
            RuleFor(x => x.Name)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("نام مجوز نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("نام مجوز باید حداقل 3 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\.]+$").When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("نام مجوز فقط می‌تواند شامل حروف، اعداد، نقطه و زیرخط باشد");

            // Description validation - optional but if provided, must be valid
            RuleFor(x => x.Description)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("توضیحات مجوز نمی‌تواند بیشتر از 500 کاراکتر باشد")
                .MinimumLength(10).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("توضیحات مجوز باید حداقل 10 کاراکتر باشد");

            // DisplayName validation - optional but if provided, must be valid
            RuleFor(x => x.DisplayName)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.DisplayName))
                .WithMessage("نام نمایشی مجوز نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.DisplayName))
                .WithMessage("نام نمایشی مجوز باید حداقل 3 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\-\s]+$").When(x => !string.IsNullOrEmpty(x.DisplayName))
                .WithMessage("نام نمایشی مجوز فقط می‌تواند شامل حروف، اعداد، خط تیره، زیرخط و فاصله باشد");

            // Category validation - optional but if provided, must be valid
            RuleFor(x => x.Category)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Category))
                .WithMessage("دسته‌بندی مجوز نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Category))
                .WithMessage("دسته‌بندی مجوز باید حداقل 2 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\-\s]+$").When(x => !string.IsNullOrEmpty(x.Category))
                .WithMessage("دسته‌بندی مجوز فقط می‌تواند شامل حروف، اعداد، خط تیره، زیرخط و فاصله باشد");

            // Module validation - optional but if provided, must be valid
            RuleFor(x => x.Module)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Module))
                .WithMessage("ماژول مجوز نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Module))
                .WithMessage("ماژول مجوز باید حداقل 2 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\-\s]+$").When(x => !string.IsNullOrEmpty(x.Module))
                .WithMessage("ماژول مجوز فقط می‌تواند شامل حروف، اعداد، خط تیره، زیرخط و فاصله باشد");

            // Resource validation - optional but if provided, must be valid
            RuleFor(x => x.Resource)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Resource))
                .WithMessage("منبع مجوز نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Resource))
                .WithMessage("منبع مجوز باید حداقل 2 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\.]+$").When(x => !string.IsNullOrEmpty(x.Resource))
                .WithMessage("منبع مجوز فقط می‌تواند شامل حروف، اعداد، نقطه و زیرخط باشد");

            // Action validation - optional but if provided, must be valid
            RuleFor(x => x.Action)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Action))
                .WithMessage("عملیات مجوز نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Action))
                .WithMessage("عملیات مجوز باید حداقل 2 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\.]+$").When(x => !string.IsNullOrEmpty(x.Action))
                .WithMessage("عملیات مجوز فقط می‌تواند شامل حروف، اعداد، نقطه و زیرخط باشد");

            // UpdatedBy validation - optional but if provided, must be valid
            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.UpdatedBy))
                .WithMessage("شناسه کاربر بروزرسانی کننده نمی‌تواند بیشتر از 100 کاراکتر باشد")
                .Matches(@"^[a-zA-Z0-9_\-\.@]+$").When(x => !string.IsNullOrEmpty(x.UpdatedBy))
                .WithMessage("شناسه کاربر بروزرسانی کننده فقط می‌تواند شامل حروف، اعداد، خط تیره، نقطه، زیرخط و @ باشد");

            // Custom validation: At least one field must be provided for update
            RuleFor(x => x)
                .Must(permission => !string.IsNullOrEmpty(permission.Name) || 
                                   !string.IsNullOrEmpty(permission.Description) || 
                                   !string.IsNullOrEmpty(permission.DisplayName) ||
                                   !string.IsNullOrEmpty(permission.Category) ||
                                   !string.IsNullOrEmpty(permission.Module) ||
                                   !string.IsNullOrEmpty(permission.Resource) ||
                                   !string.IsNullOrEmpty(permission.Action) ||
                                   permission.IsActive.HasValue)
                .WithMessage("حداقل یکی از فیلدها باید برای بروزرسانی ارائه شود");

            // Custom validation: If both Resource and Action are provided, they should be consistent
            RuleFor(x => x)
                .Must(permission => 
                {
                    if (!string.IsNullOrEmpty(permission.Resource) && !string.IsNullOrEmpty(permission.Action))
                    {
                        // Resource and Action should follow a consistent pattern
                        return permission.Resource.Contains('.') || permission.Action.Contains('.');
                    }
                    return true;
                })
                .When(x => !string.IsNullOrEmpty(x.Resource) && !string.IsNullOrEmpty(x.Action))
                .WithMessage("منبع و عملیات مجوز باید الگوی مناسبی داشته باشند");
        }
    }
} 