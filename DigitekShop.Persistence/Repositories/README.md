# Repository Layer Documentation

## Overview
This document describes the repository pattern implementation in the DigitekShop project, including recent improvements and best practices.

## Repository Structure

### GenericRepository<T>
Base repository that provides common CRUD operations for all entities.

#### Key Features:
- **CRUD Operations**: Add, Update, Delete, Get operations
- **Soft Delete Support**: Entities are marked as deleted instead of being physically removed
- **Pagination**: Built-in pagination support for large datasets
- **Async Operations**: All operations are async with CancellationToken support
- **Audit Trail**: Automatic tracking of CreatedAt and UpdatedAt timestamps

#### Recent Improvements:
1. **Fixed Delete Operations**: Resolved null reference issues in DeleteAsync, SoftDeleteAsync, and RestoreAsync methods
2. **Added Pagination**: New GetPagedAsync and GetActivePagedAsync methods
3. **Enhanced Includes**: New GetByIdWithIncludesAsync method for eager loading

### Specialized Repositories

#### ProductRepository
Extends GenericRepository<Product> with product-specific operations.

**Key Methods:**
- `GetBySKUAsync(SKU sku)` - Find product by SKU
- `GetByCategoryAsync(int categoryId)` - Get products by category with includes
- `GetInStockAsync()` - Get products with stock > 0
- `GetLowStockProductsAsync(int threshold)` - Get products with low stock
- `SearchByNameAsync(string searchTerm)` - Search products by name
- `GetProductsWithFiltersAsync()` - Advanced filtering with multiple criteria

**Recent Improvements:**
- Added Include patterns for Category and Brand
- Enhanced search functionality with related data
- Added comprehensive filtering method

#### OrderRepository
Extends GenericRepository<Order> with order-specific operations.

**Key Methods:**
- `GetByCustomerAsync(int customerId)` - Get customer orders
- `GetByStatusAsync(OrderStatus status)` - Get orders by status
- `GetOverdueOrdersAsync()` - Get orders past delivery date
- `GetTotalRevenueAsync()` - Calculate total revenue
- `GetByOrderNumberAsync(OrderNumber orderNumber)` - Find order by number

**Recent Improvements:**
- Added Include patterns for Customer and OrderItems
- Enhanced performance with proper eager loading

## Usage Examples

### Basic CRUD Operations
```csharp
// Get all active products
var products = await _productRepository.GetActiveAsync();

// Get product by ID with includes
var product = await _productRepository.GetByIdWithIncludesAsync(1);

// Add new product
var newProduct = new Product { /* ... */ };
await _productRepository.AddAsync(newProduct);

// Update product
product.Name = new ProductName("Updated Name");
await _productRepository.UpdateAsync(product);

// Soft delete product
await _productRepository.SoftDeleteAsync(1);
```

### Pagination
```csharp
// Get paginated results
var (items, totalCount) = await _productRepository.GetActivePagedAsync(
    pageNumber: 1, 
    pageSize: 10
);
```

### Advanced Filtering
```csharp
// Get products with filters
var filteredProducts = await _productRepository.GetProductsWithFiltersAsync(
    categoryId: 1,
    minPrice: 100,
    maxPrice: 1000,
    status: ProductStatus.Active
);
```

### Transaction Management
```csharp
using var unitOfWork = _unitOfWorkFactory.Create();
try
{
    await unitOfWork.BeginTransactionAsync();
    
    // Perform operations
    await unitOfWork.Products.AddAsync(product);
    await unitOfWork.Orders.AddAsync(order);
    
    await unitOfWork.SaveChangesAsync();
    await unitOfWork.CommitTransactionAsync();
}
catch
{
    await unitOfWork.RollbackTransactionAsync();
    throw;
}
```

## Best Practices

### 1. Use Includes Wisely
- Use `GetByIdWithIncludesAsync` when you need related data
- Avoid over-including data you don't need
- Consider using projection for read-only operations

### 2. Handle Transactions Properly
- Always use try-catch blocks with transactions
- Ensure proper rollback on exceptions
- Use using statements for proper disposal

### 3. Use CancellationToken
- Pass CancellationToken to all async operations
- This allows for proper cancellation of long-running operations

### 4. Leverage Soft Delete
- Use SoftDeleteAsync instead of DeleteAsync when possible
- This preserves data integrity and allows for recovery

### 5. Use Pagination for Large Datasets
- Avoid loading entire datasets into memory
- Use GetPagedAsync for better performance

## Performance Considerations

### 1. Eager Loading
- Use Include patterns strategically
- Avoid N+1 query problems
- Consider using projection for read-only operations

### 2. Indexing
- Ensure proper database indexes on frequently queried fields
- Consider composite indexes for filtered queries

### 3. Query Optimization
- Use Where clauses before Include to reduce data transfer
- Consider using AsNoTracking for read-only operations

## Recent Bug Fixes

### 1. Fixed DeleteAsync Method
**Problem:** Null reference exception when trying to delete non-existent entities
**Solution:** Proper null checking and using FindAsync instead of Local.FirstOrDefault

### 2. Fixed SoftDeleteAsync Method
**Problem:** Similar null reference issues
**Solution:** Consistent approach with proper entity retrieval

### 3. Fixed RestoreAsync Method
**Problem:** Inconsistent behavior with entity state management
**Solution:** Proper entity state tracking and updates

## Future Improvements

1. **Caching Layer**: Implement Redis caching for frequently accessed data
2. **Bulk Operations**: Add support for bulk insert/update operations
3. **Query Specifications**: Implement specification pattern for complex queries
4. **Audit Logging**: Enhanced audit trail for all operations
5. **Performance Monitoring**: Add query performance monitoring

## Testing

All repository methods should be tested with:
- Unit tests for individual methods
- Integration tests for database operations
- Performance tests for large datasets
- Transaction rollback tests

## Dependencies

- Entity Framework Core
- Microsoft.EntityFrameworkCore
- Domain entities and interfaces
- Value objects for type safety 