# UnitOfWork Pattern Documentation

## Overview
This document describes the UnitOfWork pattern implementation in the DigitekShop project, including recent improvements and best practices for transaction management.

## Architecture

### Core Components

#### IUnitOfWork Interface
The main interface that defines all UnitOfWork operations:

```csharp
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    // Repository Properties
    IProductRepository Products { get; }
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    ICategoryRepository Categories { get; }
    IBrandRepository Brands { get; }
    IReviewRepository Reviews { get; }
    IWishlistRepository Wishlists { get; }

    // Transaction Management
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    // Transaction Status
    bool HasActiveTransaction { get; }
    string? GetCurrentTransactionId();
    
    // Change Tracking
    bool HasChanges();
    Task<bool> HasChangesAsync(CancellationToken cancellationToken = default);
    
    // Context Management
    Task ResetContextAsync(CancellationToken cancellationToken = default);
    void DetachAllEntities();
    
    // Transaction Execution
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken = default);
    Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);
    
    // Generic Repository
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
}
```

#### UnitOfWork Implementation
The concrete implementation that manages:
- Database context lifecycle
- Transaction management
- Repository instantiation
- Change tracking

#### UnitOfWorkFactory
Factory pattern for creating UnitOfWork instances:

```csharp
public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
    Task<IUnitOfWork> CreateWithTransactionAsync(CancellationToken cancellationToken = default);
}
```

#### TransactionScope
Helper class for automatic transaction management:

```csharp
public class TransactionScope : IAsyncDisposable
{
    public static async Task<TransactionScope> BeginAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);
    public async Task CommitAsync(CancellationToken cancellationToken = default);
    public async Task RollbackAsync(CancellationToken cancellationToken = default);
}
```

## Usage Examples

### Basic Usage
```csharp
// Using dependency injection
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Product> CreateProductAsync(CreateProductDto dto)
    {
        var product = new Product { /* ... */ };
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }
}
```

### Manual Transaction Management
```csharp
public async Task<Order> CreateOrderWithItemsAsync(CreateOrderDto dto)
{
    try
    {
        await _unitOfWork.BeginTransactionAsync();
        
        // Create order
        var order = new Order { /* ... */ };
        await _unitOfWork.Orders.AddAsync(order);
        
        // Add order items
        foreach (var itemDto in dto.Items)
        {
            var orderItem = new OrderItem { /* ... */ };
            await _unitOfWork.Repository<OrderItem>().AddAsync(orderItem);
        }
        
        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransactionAsync();
        
        return order;
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        throw;
    }
}
```

### Using ExecuteInTransactionAsync
```csharp
public async Task<Order> CreateOrderWithItemsAsync(CreateOrderDto dto)
{
    return await _unitOfWork.ExecuteInTransactionAsync(async () =>
    {
        // Create order
        var order = new Order { /* ... */ };
        await _unitOfWork.Orders.AddAsync(order);
        
        // Add order items
        foreach (var itemDto in dto.Items)
        {
            var orderItem = new OrderItem { /* ... */ };
            await _unitOfWork.Repository<OrderItem>().AddAsync(orderItem);
        }
        
        return order;
    });
}
```

### Using TransactionScope
```csharp
public async Task<Order> CreateOrderWithItemsAsync(CreateOrderDto dto)
{
    using var transactionScope = await TransactionScope.BeginAsync(_unitOfWork);
    
    try
    {
        // Create order
        var order = new Order { /* ... */ };
        await _unitOfWork.Orders.AddAsync(order);
        
        // Add order items
        foreach (var itemDto in dto.Items)
        {
            var orderItem = new OrderItem { /* ... */ };
            await _unitOfWork.Repository<OrderItem>().AddAsync(orderItem);
        }
        
        await transactionScope.CommitAsync();
        return order;
    }
    catch
    {
        await transactionScope.RollbackAsync();
        throw;
    }
}
```

### Using UnitOfWorkFactory
```csharp
public class OrderService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    
    public OrderService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }
    
    public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
    {
        using var unitOfWork = await _unitOfWorkFactory.CreateWithTransactionAsync();
        
        try
        {
            var order = new Order { /* ... */ };
            await unitOfWork.Orders.AddAsync(order);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            
            return order;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

## Best Practices

### 1. Always Use Using Statements
```csharp
// Good
using var unitOfWork = _unitOfWorkFactory.Create();
try
{
    // operations
}
finally
{
    await unitOfWork.DisposeAsync();
}

// Better - Using TransactionScope
using var transactionScope = await TransactionScope.BeginAsync(unitOfWork);
// operations
await transactionScope.CommitAsync();
```

### 2. Handle Exceptions Properly
```csharp
try
{
    await _unitOfWork.BeginTransactionAsync();
    // operations
    await _unitOfWork.SaveChangesAsync();
    await _unitOfWork.CommitTransactionAsync();
}
catch
{
    await _unitOfWork.RollbackTransactionAsync();
    throw;
}
```

### 3. Use CancellationToken
```csharp
public async Task<Order> CreateOrderAsync(CreateOrderDto dto, CancellationToken cancellationToken = default)
{
    return await _unitOfWork.ExecuteInTransactionAsync(async () =>
    {
        var order = new Order { /* ... */ };
        await _unitOfWork.Orders.AddAsync(order, cancellationToken);
        return order;
    }, cancellationToken);
}
```

### 4. Check for Changes Before Saving
```csharp
if (_unitOfWork.HasChanges())
{
    await _unitOfWork.SaveChangesAsync();
}
```

### 5. Use Generic Repository When Needed
```csharp
// For entities without specific repositories
var genericRepo = _unitOfWork.Repository<SomeEntity>();
await genericRepo.AddAsync(entity);
```

## Recent Improvements

### 1. Added IAsyncDisposable Support
- Proper async disposal of resources
- Better integration with async/await patterns

### 2. Enhanced Transaction Management
- `HasActiveTransaction` property for status checking
- `GetCurrentTransactionId()` for transaction identification
- `ExecuteInTransactionAsync` for simplified transaction handling

### 3. Improved Change Tracking
- `HasChanges()` and `HasChangesAsync()` methods
- Better monitoring of entity state changes

### 4. Context Management
- `ResetContextAsync()` for context cleanup
- `DetachAllEntities()` for entity state management

### 5. Factory Pattern
- `IUnitOfWorkFactory` for better DI management
- `CreateWithTransactionAsync()` for immediate transaction start

### 6. TransactionScope Helper
- Automatic transaction management
- RAII pattern for resource cleanup

## Performance Considerations

### 1. Connection Pooling
- Use `IDbContextFactory` for better connection management
- Avoid long-lived contexts

### 2. Transaction Isolation
- Use appropriate isolation levels
- Keep transactions as short as possible

### 3. Batch Operations
- Use `AddRangeAsync` for multiple entities
- Consider bulk operations for large datasets

### 4. Memory Management
- Dispose UnitOfWork instances promptly
- Use `DetachAllEntities()` for large operations

## Testing

### Unit Tests
```csharp
[Test]
public async Task CreateOrder_ShouldCommitTransaction()
{
    // Arrange
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    var orderService = new OrderService(mockUnitOfWork.Object);
    
    // Act
    await orderService.CreateOrderAsync(dto);
    
    // Assert
    mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    mockUnitOfWork.Verify(uow => uow.CommitTransactionAsync(), Times.Once);
}
```

### Integration Tests
```csharp
[Test]
public async Task CreateOrder_ShouldRollbackOnException()
{
    // Arrange
    using var unitOfWork = _unitOfWorkFactory.Create();
    await unitOfWork.BeginTransactionAsync();
    
    // Act & Assert
    Assert.ThrowsAsync<Exception>(async () =>
    {
        // Simulate exception
        throw new Exception("Test exception");
    });
    
    Assert.IsFalse(unitOfWork.HasActiveTransaction);
}
```

## Dependencies

- Entity Framework Core
- Microsoft.EntityFrameworkCore
- Domain interfaces and entities
- Repository implementations

## Future Enhancements

1. **Distributed Transactions**: Support for distributed transaction scenarios
2. **Audit Logging**: Automatic audit trail for all operations
3. **Performance Monitoring**: Query performance tracking
4. **Caching Integration**: Redis caching support
5. **Event Sourcing**: Domain event publishing 