# Transaction Management با Unit of Work

## ✅ **تغییرات انجام شده:**

### 🔄 **1. به‌روزرسانی Command Handlers:**

#### قبل (مشکل):
```csharp
public CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IProductRepository productRepository,
    // ... بیشتر
)
{
    // هر Repository جداگانه SaveChanges می‌کند
    await _orderRepository.SaveChangesAsync(); // فقط Order ذخیره می‌شود
}
```

#### بعد (صحیح):
```csharp
public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
{
    _unitOfWork = unitOfWork;
}

public async Task<OrderDto> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
{
    try
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        // عملیات مختلف
        var customer = await _unitOfWork.Customers.GetByIdAsync(command.CustomerId);
        var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
        product.UpdateStock(product.StockQuantity - itemDto.Quantity);
        
        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.Products.UpdateAsync(product);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        
        return _mapper.MapToDto(order);
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        throw;
    }
}
```

### 🎯 **2. مزایای تغییرات:**

#### **مدیریت تراکنش‌ها:**
- ✅ همه تغییرات با هم ذخیره می‌شوند
- ✅ Rollback در صورت خطا
- ✅ Data Consistency

#### **کاهش وابستگی‌ها:**
- ✅ فقط یک dependency به جای چندین Repository
- ✅ کد تمیزتر و قابل نگهداری‌تر

#### **Performance:**
- ✅ کاهش تعداد SaveChanges
- ✅ بهینه‌سازی تراکنش‌ها

### 📋 **3. الگوی Transaction Management:**

```csharp
try
{
    // 1. شروع تراکنش
    await _unitOfWork.BeginTransactionAsync(cancellationToken);
    
    // 2. عملیات‌های مختلف
    await _unitOfWork.Products.UpdateAsync(product);
    await _unitOfWork.Orders.AddAsync(order);
    
    // 3. ذخیره تغییرات
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    
    // 4. تایید تراکنش
    await _unitOfWork.CommitTransactionAsync(cancellationToken);
}
catch
{
    // 5. Rollback در صورت خطا
    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
    throw;
}
```

### 🔧 **4. Query Handlers:**

Query Handlers نیازی به تراکنش ندارند (فقط خواندن):

```csharp
public class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<ProductDto> HandleAsync(GetProductQuery query, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(query.Id);
        return _mapper.MapToDto(product);
    }
}
```

### 📊 **5. مقایسه قبل و بعد:**

| جنبه | قبل | بعد |
|------|-----|-----|
| **Dependencies** | چندین Repository | یک UnitOfWork |
| **Transaction** | هر Repository جداگانه | همه با هم |
| **Rollback** | ❌ ندارد | ✅ کامل |
| **Performance** | ❌ کندتر | ✅ سریع‌تر |
| **Maintainability** | ❌ پیچیده | ✅ ساده |

### 🚀 **6. مراحل بعدی:**

1. **پیاده‌سازی UnitOfWork در Infrastructure Layer**
2. **اضافه کردن IUnitOfWork به DI Container**
3. **پیاده‌سازی Repository ها در Infrastructure**
4. **اضافه کردن Database Context**

### 📝 **7. نکات مهم:**

- **Command Handlers**: نیاز به تراکنش دارند
- **Query Handlers**: نیازی به تراکنش ندارند
- **Rollback**: همیشه در catch block انجام شود
- **CancellationToken**: در تمام متدها پاس شود 