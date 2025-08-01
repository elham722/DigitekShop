# Exception Handling Guide

## Overview
This document provides guidelines for using custom exceptions in the DigitekShop application.

## Exception Hierarchy

### Domain Exceptions (DigitekShop.Domain.Exceptions)
All domain exceptions inherit from `DomainException`:

- **ProductNotFoundException**: When a product is not found
- **CustomerNotFoundException**: When a customer is not found
- **CategoryNotFoundException**: When a category is not found
- **BrandNotFoundException**: When a brand is not found
- **OrderNotFoundException**: When an order is not found
- **InsufficientStockException**: When product stock is insufficient
- **InvalidOrderStatusException**: When order status transition is invalid
- **CustomerNotActiveException**: When customer is not active for operations
- **InvalidProductDataException**: When product data is invalid

### Application Exceptions (DigitekShop.Application.Exceptions)
Application exceptions inherit from `ApplicationException`:

- **ValidationException**: For validation failures (includes error list)
- **NotFoundException**: Generic not found exception
- **BadRequestException**: Generic bad request exception

## Usage Guidelines

### 1. Domain Layer
- Use domain exceptions for business rule violations
- Always include relevant context (IDs, names, etc.)
- Provide clear, descriptive error messages

```csharp
// Good
throw new ProductNotFoundException(productId);

// Good
throw new InsufficientStockException(productId, productName, requestedQuantity, availableQuantity);

// Avoid
throw new ArgumentException("Product not found");
```

### 2. Application Layer
- Use application exceptions for application-level concerns
- Use ValidationException for FluentValidation failures
- Use NotFoundException for generic not-found scenarios

```csharp
// Good
throw new ValidationException(validationResult);

// Good
throw new NotFoundException("Product", productId);
```

### 3. Handler Implementation
- Always use custom exceptions instead of generic .NET exceptions
- Include proper error context
- Use transaction rollback in catch blocks

```csharp
public async Task<ProductDto> HandleAsync(GetProductQuery query, CancellationToken cancellationToken = default)
{
    var product = await _unitOfWork.Products.GetByIdAsync(query.Id);
    if (product == null)
        throw new ProductNotFoundException(query.Id);

    return _mapper.Map<ProductDto>(product);
}
```

## Global Exception Handling

The application includes a `GlobalExceptionHandlerMiddleware` that:

1. **Catches all exceptions** thrown in the application
2. **Maps exceptions to HTTP status codes**:
   - Domain exceptions → 400 Bad Request
   - NotFound exceptions → 404 Not Found
   - Validation exceptions → 400 Bad Request with error list
   - Unhandled exceptions → 500 Internal Server Error
3. **Returns consistent JSON responses** using `ApiResponseDto<T>`
4. **Logs unhandled exceptions** for debugging

### Registration
Add the middleware in your `Startup.cs` or `Program.cs`:

```csharp
app.UseGlobalExceptionHandler();
```

## Response Format

All exceptions return a consistent JSON response:

```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "errors": ["Error 1", "Error 2"] // Only for ValidationException
}
```

## Testing Exceptions

When writing unit tests for handlers:

1. **Test exception scenarios**:
```csharp
[Fact]
public async Task HandleAsync_WhenProductNotFound_ThrowsProductNotFoundException()
{
    // Arrange
    var query = new GetProductQuery { Id = 1 };
    _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(1))
        .ReturnsAsync((Product)null);

    // Act & Assert
    await Assert.ThrowsAsync<ProductNotFoundException>(
        () => _handler.HandleAsync(query));
}
```

2. **Test exception properties**:
```csharp
[Fact]
public async Task HandleAsync_WhenProductNotFound_ThrowsExceptionWithCorrectId()
{
    // Arrange
    var query = new GetProductQuery { Id = 1 };
    _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(1))
        .ReturnsAsync((Product)null);

    // Act
    var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
        () => _handler.HandleAsync(query));

    // Assert
    Assert.Equal(1, exception.ProductId);
}
```

## Best Practices

1. **Be specific**: Use the most specific exception type available
2. **Include context**: Always include relevant IDs, names, or values
3. **Consistent messages**: Use consistent error message formatting
4. **Avoid generic exceptions**: Don't use `ArgumentException` or `InvalidOperationException`
5. **Transaction safety**: Always rollback transactions in catch blocks
6. **Logging**: Let the global handler handle logging of unhandled exceptions

## Common Patterns

### Not Found Pattern
```csharp
var entity = await _repository.GetByIdAsync(id);
if (entity == null)
    throw new EntityNotFoundException(id);
```

### Validation Pattern
```csharp
var validationResult = await _validator.ValidateAsync(command);
if (!validationResult.IsValid)
    throw new ValidationException(validationResult);
```

### Business Rule Pattern
```csharp
if (!entity.CanPerformOperation())
    throw new InvalidEntityStateException(entity.Id, "operation");
``` 