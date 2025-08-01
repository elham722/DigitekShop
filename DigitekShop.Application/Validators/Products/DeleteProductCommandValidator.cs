using FluentValidation;
using DigitekShop.Application.Features.Products.Commands.DeleteProduct;

namespace DigitekShop.Application.Validators.Products
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("شناسه محصول الزامی است");
        }
    }
} 