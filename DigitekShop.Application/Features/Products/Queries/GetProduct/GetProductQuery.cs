using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Features.Products.Queries.GetProduct
{
    public record GetProductQuery : IQuery<ProductDto>
    {
        public int Id { get; init; }
    }
}   