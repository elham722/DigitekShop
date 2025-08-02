# Controller Fixes Guide

## Overview
This document explains the fixes applied to resolve the controller compilation errors and response handling issues.

## Problems Identified

### 1. Response Type Mismatch
**Error**: `'SuccessResponse<ProductDto>' does not contain a definition for 'Name'`

**Root Cause**: Controllers expected direct `ProductDto` but handlers returned wrapped responses.

**Solution**: Updated controllers to handle wrapped responses correctly.

### 2. Command Response Issues
**Error**: `'CommandResponse<ProductDto>' does not contain a definition for 'Name'`

**Root Cause**: Controllers tried to access properties directly on response wrappers.

**Solution**: Access data through `.Data` property.

### 3. Unit Type Issues
**Error**: `Cannot assign void to an implicitly-typed variable`

**Root Cause**: `DeleteProductCommandHandler` used `Unit` type which was not properly defined.

**Solution**: Replaced `Unit` with `CommandResponse`.

## Fixes Applied

### 1. Controller Response Handling

#### Before (Incorrect)
```csharp
var result = await _mediator.Send(query);
_logger.LogInformation("Retrieved product: {ProductName}", result.Name);
return Ok(result);
```

#### After (Correct)
```csharp
var result = await _mediator.Send(query);
if (result == null || result.Data == null)
{
    return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));
}
_logger.LogInformation("Retrieved product: {ProductName}", result.Data.Name);
return Ok(result.Data);
```

### 2. Exception Handling

#### Added Specific Exception Catches
```csharp
catch (ProductNotFoundException ex)
{
    _logger.LogWarning("Product with ID {ProductId} not found", id);
    return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));
}
catch (ValidationException ex)
{
    _logger.LogWarning("Validation error: {Errors}", ex.Errors);
    return BadRequest(new ErrorResponse("خطا در اعتبارسنجی داده‌ها", ex.Errors));
}
catch (DuplicateEntityException ex)
{
    _logger.LogWarning("Duplicate entity error: {Message}", ex.Message);
    return BadRequest(new ErrorResponse(ex.Message));
}
```

### 3. Response Types

#### CommandResponse Structure
```csharp
public class CommandResponse<T> : BaseResponse where T : class
{
    public T? Data { get; set; }
    public int? Id { get; set; }
    public string Operation { get; set; }
    public int? AffectedRows { get; set; }
}

public class CommandResponse : BaseResponse
{
    public string Operation { get; set; }
    public int? AffectedRows { get; set; }
}
```

#### SuccessResponse Structure
```csharp
public class SuccessResponse<T> : BaseResponse where T : class
{
    public T? Data { get; set; }
    public int? TotalCount { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}
```

### 4. Handler Updates

#### DeleteProductCommandHandler
```csharp
// Before
public async Task<Unit> HandleAsync(DeleteProductCommand command, CancellationToken cancellationToken = default)

// After
public async Task<CommandResponse> HandleAsync(DeleteProductCommand command, CancellationToken cancellationToken = default)
{
    // ... implementation
    return ResponseFactory.CreateCommandSuccess("DeleteProduct", "Product deleted successfully");
}
```

### 5. Interface Updates

#### ICommandHandler
```csharp
// Before
public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand

// After
public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, CommandResponse> where TCommand : ICommand
```

## Response Flow

### 1. Query Flow
```
Controller → Mediator → QueryHandler → SuccessResponse<T> → Controller → Ok(result.Data)
```

### 2. Command Flow
```
Controller → Mediator → CommandHandler → CommandResponse<T> → Controller → Ok(result.Data)
```

### 3. Delete Flow
```
Controller → Mediator → CommandHandler → CommandResponse → Controller → Ok(SuccessResponse)
```

## Error Handling Strategy

### 1. Domain Exceptions
- `ProductNotFoundException` → 404 Not Found
- `CategoryNotFoundException` → 400 Bad Request
- `BrandNotFoundException` → 400 Bad Request
- `DuplicateEntityException` → 400 Bad Request

### 2. Application Exceptions
- `ValidationException` → 400 Bad Request
- General exceptions → 500 Internal Server Error

### 3. Logging Strategy
- **Information**: Successful operations
- **Warning**: Expected errors (not found, validation)
- **Error**: Unexpected exceptions

## Testing the Fixes

### 1. Compilation Test
```bash
dotnet build DigitekShop.Api
```

### 2. Runtime Test
```bash
dotnet run --project DigitekShop.Api
```

### 3. API Test
```http
GET /api/products/1
POST /api/products
PUT /api/products/1
DELETE /api/products/1
```

## Best Practices

### 1. Response Handling
- Always check for null responses
- Access data through `.Data` property
- Handle specific exceptions appropriately
- Log all operations and errors

### 2. Error Messages
- Use Persian error messages for user-facing errors
- Log detailed error information for debugging
- Don't expose internal error details to clients

### 3. Response Types
- Use `SuccessResponse<T>` for queries
- Use `CommandResponse<T>` for commands with data
- Use `CommandResponse` for commands without data

## Future Improvements

### 1. Response Wrapper
Consider creating a unified response wrapper:
```csharp
public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}
```

### 2. Global Exception Handler
Implement a global exception handler to centralize error handling:
```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Handle different exception types
    }
}
```

### 3. Response Caching
Consider implementing response caching for read operations:
```csharp
[ResponseCache(Duration = 300)]
[HttpGet]
public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
```

## Conclusion

The fixes ensure:
- Proper response type handling
- Consistent error handling
- Better logging and monitoring
- Type-safe operations
- Clean separation of concerns

All controllers now follow the same pattern and handle responses correctly. 