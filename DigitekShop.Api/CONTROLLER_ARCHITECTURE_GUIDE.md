# Controller Architecture Guide

## Overview
This document explains the controller architecture in the DigitekShop API, following Clean Architecture principles and CQRS pattern.

## Architecture Principles

### 1. Clean Architecture Layers
```
API Layer (Controllers)
    ↓
Application Layer (Commands/Queries)
    ↓
Domain Layer (Entities/Business Logic)
    ↓
Infrastructure Layer (Repositories/External Services)
```

### 2. CQRS Pattern
- **Commands**: For write operations (Create, Update, Delete)
- **Queries**: For read operations (Get, List, Search)

### 3. Mediator Pattern
- Controllers use `IMediator` to send Commands/Queries
- No direct dependency on Application services
- Loose coupling between layers

## Controller Structure

### Base Controller Pattern
```csharp
[ApiController]
[Route("api/[controller]")]
[CorsPolicy]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
}
```

### Key Components

#### 1. Dependencies
- **IMediator**: For sending Commands/Queries
- **ILogger**: For structured logging
- **No direct repository access**: Follows Clean Architecture

#### 2. Attributes
- **ApiController**: Enables API-specific behaviors
- **Route**: Defines API route pattern
- **CorsPolicy**: Applies CORS policy
- **ProducesResponseType**: Documents response types

#### 3. Error Handling
- **Try-catch blocks**: For exception handling
- **Structured logging**: For debugging and monitoring
- **Consistent error responses**: Using ErrorResponse DTOs

## API Endpoints

### 1. CRUD Operations

#### GET /api/products
```csharp
[HttpGet]
public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
{
    var result = await _mediator.Send(query);
    return Ok(result);
}
```

#### GET /api/products/{id}
```csharp
[HttpGet("{id:int}")]
public async Task<IActionResult> GetProduct(int id)
{
    var query = new GetProductQuery { Id = id };
    var result = await _mediator.Send(query);
    
    if (result == null)
        return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));
        
    return Ok(result);
}
```

#### POST /api/products
```csharp
[HttpPost]
public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
{
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
}
```

#### PUT /api/products/{id}
```csharp
[HttpPut("{id:int}")]
public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
{
    if (id != command.Id)
        return BadRequest(new ErrorResponse("شناسه در URL و بدنه درخواست مطابقت ندارد"));
        
    var result = await _mediator.Send(command);
    return Ok(result);
}
```

#### DELETE /api/products/{id}
```csharp
[HttpDelete("{id:int}")]
public async Task<IActionResult> DeleteProduct(int id)
{
    var command = new DeleteProductCommand { Id = id };
    var result = await _mediator.Send(command);
    
    if (!result.IsSuccess)
        return NotFound(new ErrorResponse(result.Message));
        
    return Ok(new SuccessResponse("محصول با موفقیت حذف شد"));
}
```

### 2. Business Operations

#### GET /api/products/category/{categoryId}
```csharp
[HttpGet("category/{categoryId:int}")]
public async Task<IActionResult> GetProductsByCategory(int categoryId, 
    [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
{
    var query = new GetProductsQuery
    {
        CategoryId = categoryId,
        PageNumber = pageNumber,
        PageSize = pageSize
    };
    
    var result = await _mediator.Send(query);
    return Ok(result);
}
```

#### GET /api/products/search
```csharp
[HttpGet("search")]
public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm, 
    [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
{
    if (string.IsNullOrWhiteSpace(searchTerm))
        return BadRequest(new ErrorResponse("عبارت جستجو نمی‌تواند خالی باشد"));
        
    var query = new GetProductsQuery
    {
        SearchTerm = searchTerm,
        PageNumber = pageNumber,
        PageSize = pageSize
    };
    
    var result = await _mediator.Send(query);
    return Ok(result);
}
```

#### GET /api/products/top-selling
```csharp
[HttpGet("top-selling")]
public async Task<IActionResult> GetTopSellingProducts([FromQuery] int count = 10)
{
    var query = new GetProductsQuery
    {
        PageSize = count,
        SortBy = "SalesCount",
        SortOrder = "Descending"
    };
    
    var result = await _mediator.Send(query);
    return Ok(result.Items);
}
```

#### GET /api/products/new-arrivals
```csharp
[HttpGet("new-arrivals")]
public async Task<IActionResult> GetNewArrivals([FromQuery] int count = 10)
{
    var query = new GetProductsQuery
    {
        PageSize = count,
        SortBy = "CreatedAt",
        SortOrder = "Descending"
    };
    
    var result = await _mediator.Send(query);
    return Ok(result.Items);
}
```

## Response Types

### 1. Success Responses
```csharp
// Single entity
return Ok(productDto);

// List with pagination
return Ok(pagedResultDto);

// Created resource
return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);

// Success message
return Ok(new SuccessResponse("عملیات با موفقیت انجام شد"));
```

### 2. Error Responses
```csharp
// Bad request
return BadRequest(new ErrorResponse("خطا در اعتبارسنجی داده‌ها", validationErrors));

// Not found
return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));

// Server error
return StatusCode(500, new ErrorResponse("خطا در پردازش درخواست"));
```

## Logging Strategy

### 1. Structured Logging
```csharp
_logger.LogInformation("Getting products with filters: {@Filters}", query);
_logger.LogInformation("Retrieved {Count} products", result.Items?.Count() ?? 0);
_logger.LogWarning("Product with ID {ProductId} not found", id);
_logger.LogError(ex, "Error occurred while getting product with ID: {ProductId}", id);
```

### 2. Log Levels
- **Information**: Normal operations
- **Warning**: Expected issues (not found, validation errors)
- **Error**: Unexpected exceptions
- **Debug**: Detailed debugging information

## Validation

### 1. Model Validation
```csharp
catch (ValidationException ex)
{
    _logger.LogWarning("Validation error while creating product: {Errors}", ex.Errors);
    return BadRequest(new ErrorResponse("خطا در اعتبارسنجی داده‌ها", ex.Errors));
}
```

### 2. Business Rule Validation
```csharp
if (string.IsNullOrWhiteSpace(searchTerm))
{
    _logger.LogWarning("Empty search term provided");
    return BadRequest(new ErrorResponse("عبارت جستجو نمی‌تواند خالی باشد"));
}
```

## Security Considerations

### 1. CORS Configuration
```csharp
[CorsPolicy]
public class ProductsController : ControllerBase
```

### 2. Input Validation
- All inputs are validated through Commands/Queries
- Business rules are enforced in Domain layer
- No direct database access from controllers

### 3. Error Information
- Don't expose internal errors to clients
- Use generic error messages for security
- Log detailed errors for debugging

## Testing Strategy

### 1. Unit Tests
```csharp
[Test]
public async Task GetProducts_WithValidQuery_ShouldReturnProducts()
{
    // Arrange
    var query = new GetProductsQuery { PageNumber = 1, PageSize = 10 };
    var expectedResult = new PagedResultDto<ProductListDto> { /* ... */ };
    
    _mediator.Setup(m => m.Send(query, default))
        .ReturnsAsync(expectedResult);
    
    // Act
    var result = await _controller.GetProducts(query);
    
    // Assert
    var okResult = result as OkObjectResult;
    Assert.That(okResult.Value, Is.EqualTo(expectedResult));
}
```

### 2. Integration Tests
```csharp
[Test]
public async Task GetProducts_ShouldReturnCorrectResponse()
{
    // Arrange
    var client = _factory.CreateClient();
    
    // Act
    var response = await client.GetAsync("/api/products");
    
    // Assert
    Assert.That(response.IsSuccessStatusCode, Is.True);
    var content = await response.Content.ReadAsStringAsync();
    var products = JsonSerializer.Deserialize<PagedResultDto<ProductListDto>>(content);
    Assert.That(products, Is.Not.Null);
}
```

## Best Practices

### 1. Controller Design
- Keep controllers thin
- Use dependency injection
- Follow REST conventions
- Use async/await consistently

### 2. Error Handling
- Use try-catch blocks
- Log all errors
- Return appropriate HTTP status codes
- Provide meaningful error messages

### 3. Logging
- Use structured logging
- Include relevant context
- Don't log sensitive information
- Use appropriate log levels

### 4. Documentation
- Use XML comments
- Include ProducesResponseType attributes
- Document all parameters
- Provide examples

### 5. Performance
- Use async operations
- Implement pagination
- Consider caching strategies
- Monitor response times

## Future Enhancements

### 1. Authentication & Authorization
```csharp
[Authorize]
[ApiController]
public class ProductsController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        // Implementation
    }
}
```

### 2. Rate Limiting
```csharp
[EnableRateLimiting("fixed")]
public class ProductsController : ControllerBase
```

### 3. Caching
```csharp
[ResponseCache(Duration = 300)]
[HttpGet]
public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
{
    // Implementation
}
```

### 4. API Versioning
```csharp
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : ControllerBase
```

## Conclusion

This controller architecture provides:
- Clean separation of concerns
- Testable and maintainable code
- Consistent error handling
- Proper logging and monitoring
- Scalable and extensible design

Follow these patterns for all controllers in the application to maintain consistency and quality. 