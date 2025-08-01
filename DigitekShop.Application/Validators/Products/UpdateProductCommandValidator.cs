using FluentValidation;
using DigitekShop.Application.Features.Products.Commands.UpdateProduct;

namespace DigitekShop.Application.Validators.Products
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("شناسه محصول الزامی است");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام محصول الزامی است")
                .MaximumLength(100).WithMessage("نام محصول نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("توضیحات محصول الزامی است")
                .MaximumLength(1000).WithMessage("توضیحات محصول نمی‌تواند بیشتر از 1000 کاراکتر باشد");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("قیمت باید بیشتر از صفر باشد")
                .LessThanOrEqualTo(1000000000).WithMessage("قیمت نمی‌تواند بیشتر از 1 میلیارد تومان باشد");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("موجودی نمی‌تواند منفی باشد")
                .LessThanOrEqualTo(100000).WithMessage("موجودی نمی‌تواند بیشتر از 100,000 باشد");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("دسته‌بندی محصول الزامی است");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).When(x => x.BrandId.HasValue).WithMessage("شناسه برند باید معتبر باشد");

            RuleFor(x => x.Model)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Model)).WithMessage("مدل محصول نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.Weight)
                .GreaterThan(0).When(x => x.Weight > 0).WithMessage("وزن محصول باید بیشتر از صفر باشد")
                .LessThanOrEqualTo(1000).When(x => x.Weight > 0).WithMessage("وزن محصول نمی‌تواند بیشتر از 1000 کیلوگرم باشد");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.ImageUrl)).WithMessage("آدرس تصویر نمی‌تواند بیشتر از 500 کاراکتر باشد")
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.ImageUrl)).WithMessage("آدرس تصویر باید معتبر باشد");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
} 