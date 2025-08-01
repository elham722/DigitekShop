using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Responses;

namespace DigitekShop.Application.Features.Products.Queries.GetProduct
{
    public record GetProductQuery : IQuery<SuccessResponse<ProductDto>>
    {
        public int Id { get; init; }
    }
}   