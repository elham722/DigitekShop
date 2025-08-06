using FluentValidation;
using DigitekShop.Application.DTOs.Identity;

namespace DigitekShop.Application.Validators.Identity
{
    public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.FirstName))
                .WithMessage("نام نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[\u0600-\u06FF\s]+$").When(x => !string.IsNullOrEmpty(x.FirstName))
                .WithMessage("نام فقط می‌تواند شامل حروف فارسی باشد");

            RuleFor(x => x.LastName)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.LastName))
                .WithMessage("نام خانوادگی نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[\u0600-\u06FF\s]+$").When(x => !string.IsNullOrEmpty(x.LastName))
                .WithMessage("نام خانوادگی فقط می‌تواند شامل حروف فارسی باشد");

            RuleFor(x => x.MiddleName)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.MiddleName))
                .WithMessage("نام میانی نمی‌تواند بیشتر از 50 کاراکتر باشد")
                .Matches(@"^[\u0600-\u06FF\s]+$").When(x => !string.IsNullOrEmpty(x.MiddleName))
                .WithMessage("نام میانی فقط می‌تواند شامل حروف فارسی باشد");

            RuleFor(x => x.DisplayName)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.DisplayName))
                .WithMessage("نام نمایشی نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now.AddYears(-10)).When(x => x.DateOfBirth.HasValue)
                .WithMessage("سن کاربر باید حداقل 10 سال باشد")
                .GreaterThan(DateTime.Now.AddYears(-120)).When(x => x.DateOfBirth.HasValue)
                .WithMessage("تاریخ تولد معتبر نیست");

            RuleFor(x => x.Gender)
                .MaximumLength(10).When(x => !string.IsNullOrEmpty(x.Gender))
                .WithMessage("جنسیت نمی‌تواند بیشتر از 10 کاراکتر باشد")
                .Must(BeValidGender).When(x => !string.IsNullOrEmpty(x.Gender))
                .WithMessage("جنسیت باید یکی از مقادیر 'مرد'، 'زن' یا 'سایر' باشد");

            // At least one field should be provided for update
            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage("حداقل یکی از فیلدها باید برای بروزرسانی ارائه شود");
        }

        private bool BeValidGender(string gender)
        {
            return new[] { "مرد", "زن", "سایر" }.Contains(gender);
        }

        private bool HaveAtLeastOneField(UpdateProfileDto dto)
        {
            return !string.IsNullOrEmpty(dto.FirstName) ||
                   !string.IsNullOrEmpty(dto.LastName) ||
                   !string.IsNullOrEmpty(dto.MiddleName) ||
                   !string.IsNullOrEmpty(dto.DisplayName) ||
                   dto.DateOfBirth.HasValue ||
                   !string.IsNullOrEmpty(dto.Gender);
        }
    }
} 