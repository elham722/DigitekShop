using FluentValidation;
using DigitekShop.Application.Features.Orders.Commands.CreateOrder;

namespace DigitekShop.Application.Validators.Orders
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("شناسه مشتری الزامی است");

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("سفارش باید حداقل یک آیتم داشته باشد")
                .Must(items => items != null && items.Any()).WithMessage("سفارش باید حداقل یک آیتم داشته باشد");

            RuleForEach(x => x.OrderItems).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .GreaterThan(0).WithMessage("شناسه محصول الزامی است");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("تعداد آیتم باید بیشتر از صفر باشد")
                    .LessThanOrEqualTo(1000).WithMessage("تعداد آیتم نمی‌تواند بیشتر از 1000 باشد");

                item.RuleFor(x => x.UnitPrice)
                    .GreaterThan(0).WithMessage("قیمت واحد باید بیشتر از صفر باشد");
            });

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("روش پرداخت معتبر نیست");

            RuleFor(x => x.ShippingMethod)
                .IsInEnum().WithMessage("روش ارسال معتبر نیست");

            RuleFor(x => x.ShippingStreet)
                .NotEmpty().WithMessage("آدرس ارسال الزامی است")
                .MaximumLength(200).WithMessage("آدرس ارسال نمی‌تواند بیشتر از 200 کاراکتر باشد");

            RuleFor(x => x.ShippingCity)
                .NotEmpty().WithMessage("شهر ارسال الزامی است")
                .MaximumLength(50).WithMessage("شهر ارسال نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.ShippingState)
                .NotEmpty().WithMessage("استان ارسال الزامی است")
                .MaximumLength(50).WithMessage("استان ارسال نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.ShippingPostalCode)
                .NotEmpty().WithMessage("کد پستی ارسال الزامی است")
                .Matches(@"^\d{10}$").WithMessage("کد پستی ارسال باید 10 رقم باشد");

            RuleFor(x => x.ShippingCountry)
                .NotEmpty().WithMessage("کشور ارسال الزامی است")
                .MaximumLength(50).WithMessage("کشور ارسال نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.BillingStreet)
                .NotEmpty().WithMessage("آدرس صورتحساب الزامی است")
                .MaximumLength(200).WithMessage("آدرس صورتحساب نمی‌تواند بیشتر از 200 کاراکتر باشد");

            RuleFor(x => x.BillingCity)
                .NotEmpty().WithMessage("شهر صورتحساب الزامی است")
                .MaximumLength(50).WithMessage("شهر صورتحساب نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.BillingState)
                .NotEmpty().WithMessage("استان صورتحساب الزامی است")
                .MaximumLength(50).WithMessage("استان صورتحساب نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.BillingPostalCode)
                .NotEmpty().WithMessage("کد پستی صورتحساب الزامی است")
                .Matches(@"^\d{10}$").WithMessage("کد پستی صورتحساب باید 10 رقم باشد");

            RuleFor(x => x.BillingCountry)
                .NotEmpty().WithMessage("کشور صورتحساب الزامی است")
                .MaximumLength(50).WithMessage("کشور صورتحساب نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Notes)).WithMessage("یادداشت سفارش نمی‌تواند بیشتر از 1000 کاراکتر باشد");
        }
    }
} 