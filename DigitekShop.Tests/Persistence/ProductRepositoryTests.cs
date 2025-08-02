using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Persistence.Contexts;
using DigitekShop.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitekShop.Tests.Persistence
{
    public class ProductRepositoryTests
    {
        private readonly DigitekShopDBContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DigitekShopDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DigitekShopDBContext(options);
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task GetByIdWithIncludesAsync_ShouldIncludeReviews()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var review = new Review(1, product.Id, 5, "Great Product", "Excellent quality");
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdWithIncludesAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Reviews);
            Assert.Single(result.Reviews);
            Assert.Equal(5, result.Reviews.First().Rating);
        }

        [Fact]
        public async Task GetAverageRatingAsync_ShouldReturnCorrectAverage()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var reviews = new List<Review>
            {
                new Review(1, product.Id, 5, "Great", "Excellent"),
                new Review(2, product.Id, 4, "Good", "Very good"),
                new Review(3, product.Id, 3, "Average", "Okay")
            };

            foreach (var review in reviews)
            {
                review.Approve();
            }

            await _context.Reviews.AddRangeAsync(reviews);
            await _context.SaveChangesAsync();

            // Act
            var averageRating = await _repository.GetAverageRatingAsync(product.Id);

            // Assert
            Assert.Equal(4.0m, averageRating);
        }

        [Fact]
        public async Task GetTopRatedProductsAsync_ShouldReturnOrderedProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product(new ProductName("Product 1"), "Desc 1", new Money(100, "IRR"), 10, new SKU("P1"), 1),
                new Product(new ProductName("Product 2"), "Desc 2", new Money(200, "IRR"), 10, new SKU("P2"), 1),
                new Product(new ProductName("Product 3"), "Desc 3", new Money(300, "IRR"), 10, new SKU("P3"), 1)
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Add reviews with different ratings
            var reviews = new List<Review>
            {
                new Review(1, products[0].Id, 5, "Great", "Excellent"), // Product 1: 5 stars
                new Review(2, products[1].Id, 3, "Average", "Okay"),    // Product 2: 3 stars
                new Review(3, products[2].Id, 4, "Good", "Very good")   // Product 3: 4 stars
            };

            foreach (var review in reviews)
            {
                review.Approve();
            }

            await _context.Reviews.AddRangeAsync(reviews);
            await _context.SaveChangesAsync();

            // Act
            var topRatedProducts = await _repository.GetTopRatedProductsAsync(3);

            // Assert
            var productList = topRatedProducts.ToList();
            Assert.Equal(3, productList.Count);
            Assert.Equal("Product 1", productList[0].Name.Value); // Highest rating
            Assert.Equal("Product 3", productList[1].Name.Value); // Second highest
            Assert.Equal("Product 2", productList[2].Name.Value); // Lowest rating
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
} 