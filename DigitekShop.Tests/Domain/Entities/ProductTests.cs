using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using Xunit;
using System;

namespace DigitekShop.Tests.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Product_ShouldCreateSuccessfully_WhenValidDataProvided()
        {
            // Arrange & Act
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1,
                1,
                "Test Model",
                0.5m
            );

            // Assert
            Assert.NotNull(product);
            Assert.Equal("Test Product", product.Name.Value);
            Assert.Equal("Test Description", product.Description);
            Assert.Equal(100000, product.Price.Amount);
            Assert.Equal("IRR", product.Price.Currency);
            Assert.Equal(10, product.StockQuantity);
            Assert.Equal("TEST-001", product.SKU.Value);
            Assert.Equal(1, product.CategoryId);
            Assert.Equal(1, product.BrandId);
            Assert.Equal("Test Model", product.Model);
            Assert.Equal(0.5m, product.Weight);
            Assert.Equal(ProductStatus.Active, product.Status);
        }

        [Fact]
        public void Product_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Product(
                new ProductName(""), // Empty name
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            ));
        }

        [Fact]
        public void Product_ShouldThrowException_WhenPriceIsNegative()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(-100000, "IRR"), // Negative price
                10,
                new SKU("TEST-001"),
                1
            ));
        }

        [Fact]
        public void UpdateStock_ShouldUpdateStockQuantity_WhenValidQuantityProvided()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.UpdateStock(20);

            // Assert
            Assert.Equal(20, product.StockQuantity);
        }

        [Fact]
        public void UpdateStock_ShouldThrowException_WhenNegativeQuantityProvided()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act & Assert
            Assert.Throws<ArgumentException>(() => product.UpdateStock(-5));
        }

        [Fact]
        public void UpdateStock_ShouldChangeStatusToOutOfStock_WhenStockBecomesZero()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.UpdateStock(0);

            // Assert
            Assert.Equal(0, product.StockQuantity);
            Assert.Equal(ProductStatus.OutOfStock, product.Status);
        }

        [Fact]
        public void UpdateStock_ShouldChangeStatusToActive_WhenStockBecomesAvailable()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                0,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.UpdateStock(10);

            // Assert
            Assert.Equal(10, product.StockQuantity);
            Assert.Equal(ProductStatus.Active, product.Status);
        }

        [Fact]
        public void UpdatePrice_ShouldUpdatePrice_WhenValidPriceProvided()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            var newPrice = new Money(150000, "IRR");

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(150000, product.Price.Amount);
            Assert.Equal("IRR", product.Price.Currency);
        }

        [Fact]
        public void UpdateName_ShouldUpdateName_WhenValidNameProvided()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            var newName = new ProductName("Updated Product");

            // Act
            product.UpdateName(newName);

            // Assert
            Assert.Equal("Updated Product", product.Name.Value);
        }

        [Fact]
        public void UpdateDescription_ShouldUpdateDescription_WhenValidDescriptionProvided()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.UpdateDescription("Updated Description");

            // Assert
            Assert.Equal("Updated Description", product.Description);
        }

        [Fact]
        public void UpdateDescription_ShouldSetEmptyString_WhenNullDescriptionProvided()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.UpdateDescription(null);

            // Assert
            Assert.Equal("", product.Description);
        }

        [Fact]
        public void Deactivate_ShouldChangeStatusToInactive()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.Deactivate();

            // Assert
            Assert.Equal(ProductStatus.Inactive, product.Status);
        }

        [Fact]
        public void Activate_ShouldChangeStatusToActive()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            product.Deactivate(); // First deactivate

            // Act
            product.Activate();

            // Assert
            Assert.Equal(ProductStatus.Active, product.Status);
        }

        [Fact]
        public void Discontinue_ShouldChangeStatusToDiscontinued()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.Discontinue();

            // Assert
            Assert.Equal(ProductStatus.Discontinued, product.Status);
        }

        [Fact]
        public void MarkAsComingSoon_ShouldChangeStatusToComingSoon()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.MarkAsComingSoon();

            // Assert
            Assert.Equal(ProductStatus.ComingSoon, product.Status);
        }

        [Fact]
        public void ApplyDiscount_ShouldReducePrice_WhenValidDiscountPercentageProvided()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.ApplyDiscount(20); // 20% discount

            // Assert
            Assert.Equal(80000, product.Price.Amount); // 100000 * 0.8 = 80000
        }

        [Fact]
        public void ApplyTax_ShouldIncreasePrice_WhenValidTaxRateProvided()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act
            product.ApplyTax(9); // 9% tax

            // Assert
            Assert.Equal(109000, product.Price.Amount); // 100000 * 1.09 = 109000
        }

        [Fact]
        public void IsInStock_ShouldReturnTrue_WhenStockIsAvailableAndStatusIsActive()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act & Assert
            Assert.True(product.IsInStock());
        }

        [Fact]
        public void IsInStock_ShouldReturnFalse_WhenStockIsZero()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                0,
                new SKU("TEST-001"),
                1
            );

            // Act & Assert
            Assert.False(product.IsInStock());
        }

        [Fact]
        public void IsInStock_ShouldReturnFalse_WhenStatusIsInactive()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            product.Deactivate();

            // Act & Assert
            Assert.False(product.IsInStock());
        }

        [Fact]
        public void IsExpensive_ShouldReturnTrue_WhenPriceIsOverOneMillion()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(1500000, "IRR"), // 1.5M IRR
                10,
                new SKU("TEST-001"),
                1
            );

            // Act & Assert
            Assert.True(product.IsExpensive());
        }

        [Fact]
        public void IsExpensive_ShouldReturnFalse_WhenPriceIsUnderOneMillion()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(500000, "IRR"), // 500K IRR
                10,
                new SKU("TEST-001"),
                1
            );

            // Act & Assert
            Assert.False(product.IsExpensive());
        }

        [Fact]
        public void IsLowStock_ShouldReturnTrue_WhenStockIsBelowThreshold()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                5, // Below threshold of 10
                new SKU("TEST-001"),
                1
            );

            // Act & Assert
            Assert.True(product.IsLowStock(10));
        }

        [Fact]
        public void IsLowStock_ShouldReturnFalse_WhenStockIsAboveThreshold()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                15, // Above threshold of 10
                new SKU("TEST-001"),
                1
            );

            // Act & Assert
            Assert.False(product.IsLowStock(10));
        }

        [Fact]
        public void GetDisplayName_ShouldReturnProductName()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1
            );

            // Act & Assert
            Assert.Equal("Test Product", product.GetDisplayName());
        }

        [Fact]
        public void GetFullName_ShouldReturnBrandAndModel_WhenBrandExists()
        {
            // Arrange
            var product = new Product(
                new ProductName("Test Product"),
                "Test Description",
                new Money(100000, "IRR"),
                10,
                new SKU("TEST-001"),
                1,
                1,
                "Test Model"
            );

            // Act & Assert
            Assert.Equal("Test Model", product.GetFullName()); // Brand is null in this case
        }
    }
} 