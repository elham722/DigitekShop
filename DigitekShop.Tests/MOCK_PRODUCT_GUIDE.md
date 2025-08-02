# راهنمای Mock Product Repository

## مقدمه

این فایل شامل Mock کامل برای `IProductRepository` است که برای تست‌های Product استفاده می‌شود.

## فایل‌های ساخته شده

### 1. MockProductRepository.cs
- Mock کامل برای `IProductRepository`
- پیاده‌سازی تمام متدهای `IGenericRepository` و `IProductRepository`
- داده‌های نمونه (3 محصول اپل)
- متدهای کمکی برای تست

### 2. MockProductRepositoryTests.cs
- تست‌های کامل برای Mock Repository
- تست تمام متدها
- تست داده‌های نمونه

## داده‌های نمونه

Mock repository شامل 3 محصول نمونه است:

1. **iPhone 15** - 85,000,000 تومان
2. **MacBook Air** - 75,000,000 تومان  
3. **AirPods Pro** - 8,500,000 تومان

## نحوه استفاده

```csharp
// ایجاد mock repository با داده‌های پیش‌فرض
var mockRepo = new MockProductRepository();

// ایجاد mock repository با داده‌های سفارشی
var customProducts = new List<Product> { /* محصولات سفارشی */ };
var mockRepo = new MockProductRepository(customProducts);

// استفاده در تست
var handler = new CreateProductCommandHandler(mockRepo);
var result = await handler.Handle(command, CancellationToken.None);
```

## متدهای پشتیبانی شده

### IProductRepository Methods
- `GetBySKUAsync`
- `GetByCategoryAsync`
- `GetByStatusAsync`
- `GetInStockAsync`
- `GetExpensiveProductsAsync`
- `GetLowStockProductsAsync`
- `SearchByNameAsync`
- `ExistsBySKUAsync`
- `GetCountByCategoryAsync`
- `GetProductsWithFiltersAsync`
- `GetAverageRatingAsync`
- `GetReviewCountAsync`
- `GetTopRatedProductsAsync`

### IGenericRepository Methods
- `GetByIdAsync`
- `GetByIdWithIncludesAsync`
- `GetAllAsync`
- `GetActiveAsync`
- `GetPagedAsync`
- `GetActivePagedAsync`
- `AddAsync`
- `AddRangeAsync`
- `UpdateAsync`
- `UpdateRangeAsync`
- `DeleteAsync`
- `DeleteRangeAsync`
- `SoftDeleteAsync`
- `RestoreAsync`
- `ExistsAsync`
- `ExistsActiveAsync`
- `GetTotalCountAsync`
- `GetActiveCountAsync`

## متدهای کمکی

```csharp
// پاک کردن تمام داده‌ها
mockRepo.Clear();

// اضافه کردن محصول جدید
mockRepo.AddProduct(newProduct);

// دریافت تمام محصولات
var products = mockRepo.GetAllProducts();
```

## اجرای تست‌ها

```bash
# اجرای تست‌های Mock Repository
dotnet test DigitekShop.Tests --filter "MockProductRepository"

# اجرای تمام تست‌ها
dotnet test DigitekShop.Tests
```

## ویژگی‌های کلیدی

✅ **Mock کامل** - تمام متدهای interface پیاده‌سازی شده  
✅ **داده‌های نمونه** - 3 محصول برای تست  
✅ **تست‌های کامل** - پوشش تمام متدها  
✅ **متدهای کمکی** - برای تست‌های سفارشی  
✅ **Async Support** - پشتیبانی از async/await  

حالا می‌تونید از این Mock برای تست‌های Product استفاده کنید! 🚀 