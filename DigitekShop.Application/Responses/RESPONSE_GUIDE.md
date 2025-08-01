# Custom Response System Guide

## Overview
This document provides guidelines for using the custom response system in the DigitekShop application.

## Response Hierarchy

### Base Response
All responses inherit from `BaseResponse` which provides common properties:
- `Success`: Boolean indicating operation success
- `Message`: Human-readable message
- `Timestamp`: UTC timestamp of response
- `RequestId`: Correlation ID for request tracking

### Response Types

#### 1. SuccessResponse<T>
Used for successful query operations that return data.

**Properties:**
- `Data`: The actual data being returned
- `TotalCount`: Total number of items (for paged results)
- `PageNumber`: Current page number (for paged results)
- `PageSize`: Number of items per page (for paged results)

**Usage:**
```csharp
// Simple success response
return ResponseFactory.CreateSuccess(productDto, "Product retrieved successfully");

// Paged success response
return ResponseFactory.CreatePagedSuccess(
    products, 
    totalCount, 
    pageNumber, 
    pageSize, 
    "Products retrieved successfully");
```

#### 2. CommandResponse<T>
Used for command operations (Create, Update, Delete).

**Properties:**
- `Data`: Optional data returned from command
- `Id`: ID of the created/updated entity
- `Operation`: Name of the operation performed
- `AffectedRows`: Number of rows affected

**Usage:**
```csharp
// Command with data and ID
return ResponseFactory.CreateCommandWithDataAndId(
    productDto, 
    product.Id, 
    "CreateProduct", 
    "Product created successfully");

// Simple command
return ResponseFactory.CreateCommand("DeleteProduct", "Product deleted successfully");
```

#### 3. ErrorResponse
Used for error scenarios (handled by Global Exception Handler).

**Properties:**
- `Errors`: List of error messages
- `ErrorCode`: Machine-readable error code
- `Details`: Additional error details

**Error Codes:**
- `VALIDATION_ERROR`: Validation failures
- `NOT_FOUND`: Entity not found
- `DUPLICATE_ENTITY`: Duplicate entity
- `BUSINESS_RULE_VIOLATION`: Business rule violations
- `BAD_REQUEST`: Invalid request
- `INTERNAL_ERROR`: Unexpected errors

## Response Factory

The `ResponseFactory` provides static methods to create responses:

### Success Responses
```csharp
// Simple success
ResponseFactory.CreateSuccess<T>(data, message)

// Paged success
ResponseFactory.CreatePagedSuccess<T>(data, totalCount, pageNumber, pageSize, message)

// From existing DTOs
ResponseFactory.FromApiResponse<T>(apiResponse)
ResponseFactory.FromPagedResult<T>(pagedResult, message)
```

### Command Responses
```csharp
// Simple command
ResponseFactory.CreateCommand<T>(operation, message)

// Command with data
ResponseFactory.CreateCommandWithData<T>(data, operation, message)

// Command with ID
ResponseFactory.CreateCommandWithId<T>(id, operation, message)

// Command with data and ID
ResponseFactory.CreateCommandWithDataAndId<T>(data, id, operation, message)
```

### Error Responses
```csharp
// Generic error
ResponseFactory.CreateError(message, errorCode)

// Validation error
ResponseFactory.CreateValidationError(errors, message)

// Not found error
ResponseFactory.CreateNotFound(entityName, key)

// Duplicate error
ResponseFactory.CreateDuplicate(entityName, propertyName, value)

// Business rule violation
ResponseFactory.CreateBusinessRuleViolation(message)
```

## Response Wrapper

The `ResponseWrapper` enriches responses with additional context:

```csharp
// Wrap any response with context
ResponseWrapper.Wrap(response, httpContext)

// Convenience methods
ResponseWrapper.WrapSuccess<T>(data, message, httpContext)
ResponseWrapper.WrapPagedSuccess<T>(data, totalCount, pageNumber, pageSize, message, httpContext)
ResponseWrapper.WrapCommand<T>(operation, message, httpContext)
ResponseWrapper.WrapCommandWithData<T>(data, operation, message, httpContext)
ResponseWrapper.WrapCommandWithId<T>(id, operation, message, httpContext)
ResponseWrapper.WrapCommandWithDataAndId<T>(data, id, operation, message, httpContext)
```

## Implementation Examples

### Query Handler
```csharp
public class GetProductQueryHandler : IQueryHandler<GetProductQuery, SuccessResponse<ProductDto>>
{
    public async Task<SuccessResponse<ProductDto>> HandleAsync(GetProductQuery query, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(query.Id);
        if (product == null)
            throw new ProductNotFoundException(query.Id);

        var productDto = _mapper.Map<ProductDto>(product);
        return ResponseFactory.CreateSuccess(productDto, "Product retrieved successfully");
    }
}
```

### Command Handler
```csharp
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CommandResponse<ProductDto>>
{
    public async Task<CommandResponse<ProductDto>> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        // ... business logic ...

        var productDto = _mapper.Map<ProductDto>(createdProduct);
        return ResponseFactory.CreateCommandWithDataAndId(
            productDto, 
            createdProduct.Id, 
            "CreateProduct", 
            "Product created successfully");
    }
}
```

### Paged Query Handler
```csharp
public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, SuccessResponse<List<ProductDto>>>
{
    public async Task<SuccessResponse<List<ProductDto>>> HandleAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var (products, totalCount) = await _unitOfWork.Products.GetPagedAsync(query.PageNumber, query.PageSize);
        var productDtos = _mapper.Map<List<ProductDto>>(products);
        
        return ResponseFactory.CreatePagedSuccess(
            productDtos, 
            totalCount, 
            query.PageNumber, 
            query.PageSize, 
            "Products retrieved successfully");
    }
}
```

## Response Format Examples

### Success Response
```json
{
  "success": true,
  "message": "Product retrieved successfully",
  "timestamp": "2024-01-15T10:30:00Z",
  "requestId": "trace-123",
  "data": {
    "id": 1,
    "name": "Sample Product",
    "price": 100000
  }
}
```

### Paged Success Response
```json
{
  "success": true,
  "message": "Products retrieved successfully",
  "timestamp": "2024-01-15T10:30:00Z",
  "requestId": "trace-123",
  "data": [
    {
      "id": 1,
      "name": "Product 1"
    },
    {
      "id": 2,
      "name": "Product 2"
    }
  ],
  "totalCount": 50,
  "pageNumber": 1,
  "pageSize": 10
}
```

### Command Response
```json
{
  "success": true,
  "message": "Product created successfully",
  "timestamp": "2024-01-15T10:30:00Z",
  "requestId": "trace-123",
  "data": {
    "id": 1,
    "name": "New Product",
    "price": 100000
  },
  "id": 1,
  "operation": "CreateProduct",
  "affectedRows": 1
}
```

### Error Response
```json
{
  "success": false,
  "message": "Product with ID 1 was not found",
  "timestamp": "2024-01-15T10:30:00Z",
  "requestId": "trace-123",
  "errors": ["Product with ID 1 was not found"],
  "errorCode": "NOT_FOUND",
  "details": null
}
```

## Best Practices

### 1. Use Appropriate Response Types
- Use `SuccessResponse<T>` for queries
- Use `CommandResponse<T>` for commands
- Let the Global Exception Handler create `ErrorResponse`

### 2. Provide Meaningful Messages
```csharp
// Good
return ResponseFactory.CreateSuccess(data, "Product retrieved successfully");

// Avoid
return ResponseFactory.CreateSuccess(data, "OK");
```

### 3. Include Operation Names
```csharp
// Good
return ResponseFactory.CreateCommandWithData(data, "UpdateProduct", "Product updated successfully");

// Avoid
return ResponseFactory.CreateCommandWithData(data, "Update", "Success");
```

### 4. Use Error Codes Consistently
- `VALIDATION_ERROR`: For validation failures
- `NOT_FOUND`: For missing entities
- `DUPLICATE_ENTITY`: For duplicate entries
- `BUSINESS_RULE_VIOLATION`: For business rule violations
- `BAD_REQUEST`: For invalid requests
- `INTERNAL_ERROR`: For unexpected errors

### 5. Handle Paging Properly
```csharp
// Always include paging information for paged results
return ResponseFactory.CreatePagedSuccess(
    data, 
    totalCount, 
    pageNumber, 
    pageSize, 
    message);
```

## Migration from Existing DTOs

### From ApiResponseDto
```csharp
// Old
return new ApiResponseDto<ProductDto> { Data = productDto, Success = true };

// New
return ResponseFactory.CreateSuccess(productDto, "Product retrieved successfully");
```

### From PagedResultDto
```csharp
// Old
return new PagedResultDto<ProductDto> { Items = products, TotalCount = totalCount };

// New
return ResponseFactory.CreatePagedSuccess(products, totalCount, pageNumber, pageSize);
```

## Testing Responses

### Unit Testing
```csharp
[Fact]
public async Task HandleAsync_WhenProductExists_ReturnsSuccessResponse()
{
    // Arrange
    var query = new GetProductQuery { Id = 1 };
    var product = new Product { Id = 1, Name = "Test Product" };
    _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(1))
        .ReturnsAsync(product);

    // Act
    var result = await _handler.HandleAsync(query);

    // Assert
    Assert.True(result.Success);
    Assert.Equal("Product retrieved successfully", result.Message);
    Assert.NotNull(result.Data);
    Assert.Equal(1, result.Data.Id);
}
```

### Integration Testing
```csharp
[Fact]
public async Task GetProduct_WhenProductExists_ReturnsSuccessResponse()
{
    // Arrange
    var product = await CreateTestProduct();

    // Act
    var response = await _client.GetAsync($"/api/products/{product.Id}");

    // Assert
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<SuccessResponse<ProductDto>>(content);
    
    Assert.True(result.Success);
    Assert.Equal(product.Id, result.Data.Id);
}
```

## Benefits

1. **Consistency**: All responses follow the same structure
2. **Rich Metadata**: Includes timestamps, request IDs, and operation details
3. **Type Safety**: Strongly typed responses prevent runtime errors
4. **Flexibility**: Different response types for different scenarios
5. **Maintainability**: Centralized response creation logic
6. **Debugging**: Request IDs help with tracing and debugging
7. **Documentation**: Self-documenting response structure 