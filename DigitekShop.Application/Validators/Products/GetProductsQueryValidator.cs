using FluentValidation;
using DigitekShop.Application.Features.Products.Queries.GetProducts;

namespace DigitekShop.Application.Validators.Products
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("شماره صفحه باید بیشتر از صفر باشد")
                .LessThanOrEqualTo(10000).WithMessage("شماره صفحه نمی‌تواند بیشتر از 10000 باشد");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("اندازه صفحه باید بیشتر از صفر باشد")
                .LessThanOrEqualTo(100).WithMessage("اندازه صفحه نمی‌تواند بیشتر از 100 باشد");

            RuleFor(x => x.SearchTerm)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.SearchTerm)).WithMessage("عبارت جستجو نمی‌تواند بیشتر از 100 کاراکتر باشد");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).When(x => x.CategoryId.HasValue).WithMessage("شناسه دسته‌بندی باید معتبر باشد");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).When(x => x.BrandId.HasValue).WithMessage("شناسه برند باید معتبر باشد");

            RuleFor(x => x.MinPrice)
                .GreaterThan(0).When(x => x.MinPrice.HasValue).WithMessage("حداقل قیمت باید بیشتر از صفر باشد")
                .LessThanOrEqualTo(1000000000).When(x => x.MinPrice.HasValue).WithMessage("حداقل قیمت نمی‌تواند بیشتر از 1 میلیارد تومان باشد");

            RuleFor(x => x.MaxPrice)
                .GreaterThan(0).When(x => x.MaxPrice.HasValue).WithMessage("حداکثر قیمت باید بیشتر از صفر باشد")
                .LessThanOrEqualTo(1000000000).When(x => x.MaxPrice.HasValue).WithMessage("حداکثر قیمت نمی‌تواند بیشتر از 1 میلیارد تومان باشد");

            RuleFor(x => x)
                .Must(BeValidPriceRange).WithMessage("حداکثر قیمت باید بیشتر از حداقل قیمت باشد");

            RuleFor(x => x.SortBy)
                .Must(BeValidSortBy).When(x => !string.IsNullOrEmpty(x.SortBy)).WithMessage("فیلد مرتب‌سازی معتبر نیست");
        }

        private bool BeValidPriceRange(GetProductsQuery query)
        {
            if (query.MinPrice.HasValue && query.MaxPrice.HasValue)
            {
                return query.MaxPrice.Value >= query.MinPrice.Value;
            }
            return true;
        }

        private bool BeValidSortBy(string sortBy)
        {
            var validSortFields = new[] { "name", "price", "stock", "createdat" };
            return validSortFields.Contains(sortBy.ToLower());
        }
    }
} 