# Repository & Unit of Work Pattern

## 🏗️ **ساختار فعلی:**

### Repository Pattern
```
IGenericRepository<T>
├── IProductRepository
├── ICustomerRepository
├── IOrderRepository
├── ICategoryRepository
├── IBrandRepository
├── IReviewRepository
└── IWishlistRepository
```

### Unit of Work Pattern
```
IUnitOfWork
├── Products (IProductRepository)
├── Customers (ICustomerRepository)
├── Orders (IOrderRepository)
├── Categories (ICategoryRepository)
├── Brands (IBrandRepository)
├── Reviews (IReviewRepository)
└── Wishlists (IWishlistRepository)
```

## 🎯 **مزایای Unit of Work:**

### 1. **مدیریت تراکنش‌ها**
```csharp
// قبل (مشکل): هر Repository جداگانه ذخیره می‌کند
await _orderRepository.AddAsync(order);
await _orderRepository.SaveChangesAsync(); // فقط Order ذخیره می‌شود

// بعد (صحیح): همه تغییرات با هم ذخیره می‌شوند
_unitOfWork.Orders.AddAsync(order);
_unitOfWork.Products.UpdateAsync(product);
await _unitOfWork.SaveChangesAsync(); // همه تغییرات با هم
```

### 2. **Rollback در صورت خطا**
```csharp
try
{
    await _unitOfWork.BeginTransactionAsync();
    
    // عملیات مختلف
    _unitOfWork.Orders.AddAsync(order);
    _unitOfWork.Products.UpdateAsync(product);
    
    await _unitOfWork.CommitTransactionAsync();
}
catch
{
    await _unitOfWork.RollbackTransactionAsync();
    throw;
}
```

### 3. **کاهش وابستگی‌ها**
```csharp
// قبل: نیاز به چندین Repository
public CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IProductRepository productRepository,
    // ... بیشتر
)

// بعد: فقط یک UnitOfWork
public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
{
    _unitOfWork = unitOfWork;
}
```

## 📋 **نحوه استفاده در Handlers:**

### مثال: CreateOrderCommandHandler
```csharp
public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly OrderMappingProfile _mapper;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, OrderMappingProfile mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDto> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // بررسی مشتری
            var customer = await _unitOfWork.Customers.GetByIdAsync(command.CustomerId);
            if (customer == null)
                throw new CustomerNotFoundException(command.CustomerId);

            // ایجاد سفارش
            var order = new Order(/* ... */);

            // اضافه کردن آیتم‌ها و کاهش موجودی
            foreach (var itemDto in command.OrderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                product.UpdateStock(product.StockQuantity - itemDto.Quantity);
                
                var orderItem = new OrderItem(/* ... */);
                order.AddOrderItem(orderItem);
            }

            // ذخیره همه تغییرات
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.MapToDto(order);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

## 🔧 **پیاده‌سازی در Infrastructure:**

### Entity Framework Implementation
```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _transaction;

    public IProductRepository Products { get; }
    public ICustomerRepository Customers { get; }
    public IOrderRepository Orders { get; }
    // ... سایر Repositories

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Products = new ProductRepository(context);
        Customers = new CustomerRepository(context);
        Orders = new OrderRepository(context);
        // ... سایر Repositories
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _transaction?.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _transaction?.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}
```

## ✅ **نتیجه‌گیری:**

Unit of Work pattern برای پروژه شما **ضروری** است زیرا:

1. **مدیریت تراکنش‌ها**: همه تغییرات با هم ذخیره می‌شوند
2. **Rollback**: در صورت خطا، همه تغییرات لغو می‌شوند
3. **کاهش وابستگی‌ها**: فقط یک dependency به جای چندین Repository
4. **Performance**: کاهش تعداد SaveChanges
5. **Data Consistency**: اطمینان از سازگاری داده‌ها 