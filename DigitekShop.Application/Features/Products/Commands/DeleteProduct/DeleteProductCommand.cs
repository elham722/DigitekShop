using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand : ICommand
    {
        public int Id { get; init; }
    }
} 