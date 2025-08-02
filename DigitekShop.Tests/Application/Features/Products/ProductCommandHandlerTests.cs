using DigitekShop.Application.Features.Products.Commands.CreateProduct;
using DigitekShop.Application.Features.Products.Commands.UpdateProduct;
using DigitekShop.Application.Features.Products.Commands.DeleteProduct;
using DigitekShop.Application.Features.Products.Queries.GetProducts;
using DigitekShop.Application.Features.Products.Queries.GetProduct;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using DigitekShop.Tests.Mocks;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace DigitekShop.Tests.Application.Features.Products
{
    public class ProductCommandHandlerTests
    {
        private readonly MockProductRepository _mockRepository;

        public ProductCommandHandlerTests()
        {
            _mockRepository = new MockProductRepository();
        }

        [Fact]
        public async Task CreateProductCommandHandler_ShouldCreateProduct_WhenValidDataProvided()
        {
            // Arrange
            var handler = new CreateProductCommandHandler(_mockRepository);
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100000,
                Currency = "IRR",
                StockQuantity = 10,
                SKU = "TEST-001",
                CategoryId = 1,
                BrandId = 1,
                Model = "Test Model",
                Weight = 0.5m
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Test Product", result.Data.Name);
            Assert.Equal(100000, result.Data.Price);
            Assert.Equal(10, result.Data.StockQuantity);
        }

        [Fact]
        public async Task CreateProductCommandHandler_ShouldFail_WhenDuplicateSKU()
        {
            // Arrange
            var handler = new CreateProductCommandHandler(_mockRepository);
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100000,
                Currency = "IRR",
                StockQuantity = 10,
                SKU = "PHONE-APPLE-IP15", // This SKU already exists in mock data
                CategoryId = 1,
                BrandId = 1
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("SKU", result.Message);
        }

        [Fact]
        public async Task UpdateProductCommandHandler_ShouldUpdateProduct_WhenValidDataProvided()
        {
            // Arrange
            var handler = new UpdateProductCommandHandler(_mockRepository);
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150000,
                Currency = "IRR",
                StockQuantity = 15,
                SKU = "PHONE-APPLE-IP15",
                CategoryId = 1,
                BrandId = 1,
                Model = "Updated Model",
                Weight = 0.6m
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Updated Product", result.Data.Name);
            Assert.Equal(150000, result.Data.Price);
            Assert.Equal(15, result.Data.StockQuantity);
        }

        [Fact]
        public async Task UpdateProductCommandHandler_ShouldFail_WhenProductNotFound()
        {
            // Arrange
            var handler = new UpdateProductCommandHandler(_mockRepository);
            var command = new UpdateProductCommand
            {
                Id = 999, // Non-existent ID
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150000,
                Currency = "IRR",
                StockQuantity = 15,
                SKU = "TEST-999",
                CategoryId = 1,
                BrandId = 1
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("not found", result.Message);
        }

        [Fact]
        public async Task DeleteProductCommandHandler_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var handler = new DeleteProductCommandHandler(_mockRepository);
            var command = new DeleteProductCommand { Id = 1 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            
            // Verify product is deleted
            var deletedProduct = await _mockRepository.GetByIdAsync(1);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task DeleteProductCommandHandler_ShouldFail_WhenProductNotFound()
        {
            // Arrange
            var handler = new DeleteProductCommandHandler(_mockRepository);
            var command = new DeleteProductCommand { Id = 999 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("not found", result.Message);
        }

        [Fact]
        public async Task GetProductsQueryHandler_ShouldReturnProducts_WhenValidQuery()
        {
            // Arrange
            var handler = new GetProductsQueryHandler(_mockRepository);
            var query = new GetProductsQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Items);
            Assert.True(result.Data.Items.Any());
            Assert.Equal(3, result.Data.TotalCount); // 3 products in mock data
        }

        [Fact]
        public async Task GetProductsQueryHandler_ShouldFilterByCategory_WhenCategoryIdProvided()
        {
            // Arrange
            var handler = new GetProductsQueryHandler(_mockRepository);
            var query = new GetProductsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                CategoryId = 1
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Items);
            Assert.True(result.Data.Items.All(p => p.CategoryId == 1));
        }

        [Fact]
        public async Task GetProductsQueryHandler_ShouldSearchByName_WhenSearchTermProvided()
        {
            // Arrange
            var handler = new GetProductsQueryHandler(_mockRepository);
            var query = new GetProductsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = "iPhone"
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Items);
            Assert.True(result.Data.Items.All(p => p.Name.Contains("iPhone")));
        }

        [Fact]
        public async Task GetProductQueryHandler_ShouldReturnProduct_WhenValidId()
        {
            // Arrange
            var handler = new GetProductQueryHandler(_mockRepository);
            var query = new GetProductQuery { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("iPhone 15", result.Data.Name);
        }

        [Fact]
        public async Task GetProductQueryHandler_ShouldFail_WhenProductNotFound()
        {
            // Arrange
            var handler = new GetProductQueryHandler(_mockRepository);
            var query = new GetProductQuery { Id = 999 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("not found", result.Message);
        }

        [Fact]
        public async Task GetProductsQueryHandler_ShouldFilterByPriceRange_WhenMinMaxPriceProvided()
        {
            // Arrange
            var handler = new GetProductsQueryHandler(_mockRepository);
            var query = new GetProductsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                MinPrice = 50000000, // 50M IRR
                MaxPrice = 90000000  // 90M IRR
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Items);
            Assert.True(result.Data.Items.All(p => p.Price >= 50000000 && p.Price <= 90000000));
        }

        [Fact]
        public async Task GetProductsQueryHandler_ShouldSortByPrice_WhenSortByPriceProvided()
        {
            // Arrange
            var handler = new GetProductsQueryHandler(_mockRepository);
            var query = new GetProductsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = "Price",
                SortOrder = "Ascending"
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Items);
            
            var products = result.Data.Items.ToList();
            for (int i = 1; i < products.Count; i++)
            {
                Assert.True(products[i].Price >= products[i - 1].Price);
            }
        }
    }
} 