using DigitekShop.Tests.Mocks;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace DigitekShop.Tests.Mocks
{
    public class MockProductRepositoryTests
    {
        [Fact]
        public void MockProductRepository_ShouldInitializeWithSampleData()
        {
            // Arrange & Act
            var mockRepo = new MockProductRepository();

            // Assert
            var products = mockRepo.GetAllProducts();
            Assert.Equal(3, products.Count);
            Assert.Contains(products, p => p.Name.Value == "iPhone 15");
            Assert.Contains(products, p => p.Name.Value == "MacBook Air");
            Assert.Contains(products, p => p.Name.Value == "AirPods Pro");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenValidId()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var product = await mockRepo.GetByIdAsync(1);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
            Assert.Equal("iPhone 15", product.Name.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenInvalidId()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var product = await mockRepo.GetByIdAsync(999);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.GetAllAsync();

            // Assert
            Assert.Equal(3, products.Count());
        }

        [Fact]
        public async Task GetActiveAsync_ShouldReturnOnlyActiveProducts()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.GetActiveAsync();

            // Assert
            Assert.True(products.All(p => p.Status == DigitekShop.Domain.Enums.ProductStatus.Active));
        }

        [Fact]
        public async Task SearchByNameAsync_ShouldReturnMatchingProducts()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.SearchByNameAsync("iPhone");

            // Assert
            Assert.Single(products);
            Assert.Equal("iPhone 15", products.First().Name.Value);
        }

        [Fact]
        public async Task SearchByNameAsync_ShouldReturnEmpty_WhenNoMatch()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.SearchByNameAsync("NonExistent");

            // Assert
            Assert.Empty(products);
        }

        [Fact]
        public async Task GetByCategoryAsync_ShouldReturnProductsInCategory()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.GetByCategoryAsync(1);

            // Assert
            Assert.True(products.All(p => p.CategoryId == 1));
        }

        [Fact]
        public async Task GetInStockAsync_ShouldReturnOnlyInStockProducts()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.GetInStockAsync();

            // Assert
            Assert.True(products.All(p => p.IsInStock()));
        }

        [Fact]
        public async Task GetExpensiveProductsAsync_ShouldReturnExpensiveProducts()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.GetExpensiveProductsAsync();

            // Assert
            Assert.True(products.All(p => p.IsExpensive()));
        }

        [Fact]
        public async Task GetLowStockProductsAsync_ShouldReturnLowStockProducts()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.GetLowStockProductsAsync(10);

            // Assert
            Assert.True(products.All(p => p.IsLowStock(10)));
        }

        [Fact]
        public async Task ExistsBySKUAsync_ShouldReturnTrue_WhenSKUExists()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var exists = await mockRepo.ExistsBySKUAsync(new DigitekShop.Domain.ValueObjects.SKU("PHONE-APPLE-IP15"));

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsBySKUAsync_ShouldReturnFalse_WhenSKUDoesNotExist()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var exists = await mockRepo.ExistsBySKUAsync(new DigitekShop.Domain.ValueObjects.SKU("NON-EXISTENT-SKU"));

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task GetCountByCategoryAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var count = await mockRepo.GetCountByCategoryAsync(1);

            // Assert
            Assert.Equal(3, count); // All 3 products are in category 1
        }

        [Fact]
        public async Task GetProductsWithFiltersAsync_ShouldFilterByCategory()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.GetProductsWithFiltersAsync(categoryId: 1);

            // Assert
            Assert.True(products.All(p => p.CategoryId == 1));
        }

        [Fact]
        public async Task GetProductsWithFiltersAsync_ShouldFilterByPriceRange()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            var products = await mockRepo.GetProductsWithFiltersAsync(minPrice: 50000000, maxPrice: 90000000);

            // Assert
            Assert.True(products.All(p => p.Price.Amount >= 50000000 && p.Price.Amount <= 90000000));
        }

        [Fact]
        public void Clear_ShouldRemoveAllProducts()
        {
            // Arrange
            var mockRepo = new MockProductRepository();

            // Act
            mockRepo.Clear();

            // Assert
            var products = mockRepo.GetAllProducts();
            Assert.Empty(products);
        }

        [Fact]
        public void AddProduct_ShouldAddNewProduct()
        {
            // Arrange
            var mockRepo = new MockProductRepository();
            var newProduct = new DigitekShop.Domain.Entities.Product(
                new DigitekShop.Domain.ValueObjects.ProductName("Test Product"),
                "Test Description",
                new DigitekShop.Domain.ValueObjects.Money(100000, "IRR"),
                10,
                new DigitekShop.Domain.ValueObjects.SKU("TEST-001"),
                1
            );

            // Act
            mockRepo.AddProduct(newProduct);

            // Assert
            var products = mockRepo.GetAllProducts();
            Assert.Equal(4, products.Count);
            Assert.Contains(products, p => p.Name.Value == "Test Product");
        }
    }
} 