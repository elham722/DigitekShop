using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Responses;

namespace DigitekShop.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand : ICommand<CommandResponse<ProductDto>>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int StockQuantity { get; init; }
        public string SKU { get; init; } = string.Empty;
        public int CategoryId { get; init; }
        public int? BrandId { get; init; }
        public string Model { get; init; } = string.Empty;
        public decimal Weight { get; init; }
        public string ImageUrl { get; init; } = string.Empty;
    }
} 