# راهنمای تست Product - DigitekShop

## مقدمه

این راهنما توضیح می‌دهد که چگونه از سیستم تست Product استفاده کنید و شامل mock repository و تست‌های کامل است.

## فایل‌های تست

### 1. MockProductRepository.cs
Mock کامل برای `IProductRepository` که شامل:
- تمام متدهای `IGenericRepository`
- تمام متدهای خاص `IProductRepository`
- داده‌های نمونه برای تست
- متدهای کمکی برای تست

### 2. ProductCommandHandlerTests.cs
تست‌های کامل برای Command Handlers:
- CreateProductCommandHandler
- UpdateProductCommandHandler
- DeleteProductCommandHandler
- GetProductsQueryHandler
- GetProductQueryHandler

### 3. ProductTests.cs
تست‌های Domain Entity برای Product:
- تست ساخت Product
- تست متدهای Business Logic
- تست Value Objects
- تست Validation

## نحوه استفاده

### 1. اجرای تست‌ها
```bash
# اجرای تمام تست‌ها
dotnet test

# اجرای تست‌های Product فقط
dotnet test --filter "Product"

# اجرای تست‌های خاص
dotnet test --filter "CreateProductCommandHandler"
```

### 2. استفاده از Mock Repository
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

### 3. تست Command Handlers
```csharp
[Fact]
public async Task CreateProduct_ShouldSucceed_WhenValidData()
{
    // Arrange
    var mockRepo = new MockProductRepository();
    var handler = new CreateProductCommandHandler(mockRepo);
    var command = new CreateProductCommand { /* داده‌های معتبر */ };

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
}
```

## ویژگی‌های Mock Repository

### داده‌های نمونه
Mock repository شامل 3 محصول نمونه است:
1. **iPhone 15** - 85,000,000 تومان
2. **MacBook Air** - 75,000,000 تومان
3. **AirPods Pro** - 8,500,000 تومان

### متدهای پشتیبانی شده
- تمام متدهای CRUD
- فیلتر و جستجو
- صفحه‌بندی
- مرتب‌سازی
- بررسی موجودی
- محاسبه امتیاز

### متدهای کمکی
```csharp
// پاک کردن تمام داده‌ها
mockRepo.Clear();

// اضافه کردن محصول جدید
mockRepo.AddProduct(newProduct);

// دریافت تمام محصولات
var products = mockRepo.GetAllProducts();
```

## تست‌های موجود

### Command Handler Tests
- ✅ ایجاد محصول جدید
- ✅ به‌روزرسانی محصول
- ✅ حذف محصول
- ✅ دریافت لیست محصولات
- ✅ دریافت محصول خاص
- ✅ فیلتر بر اساس دسته‌بندی
- ✅ جستجو بر اساس نام
- ✅ فیلتر بر اساس قیمت
- ✅ مرتب‌سازی

### Domain Entity Tests
- ✅ ساخت Product
- ✅ Validation
- ✅ به‌روزرسانی موجودی
- ✅ تغییر قیمت
- ✅ تغییر نام و توضیحات
- ✅ تغییر وضعیت (فعال/غیرفعال)
- ✅ اعمال تخفیف و مالیات
- ✅ بررسی موجودی
- ✅ بررسی قیمت بالا
- ✅ بررسی موجودی کم

## بهترین شیوه‌ها

### 1. نام‌گذاری تست‌ها
```csharp
[Fact]
public async Task MethodName_ShouldDoSomething_WhenCondition()
{
    // تست
}
```

### 2. الگوی AAA
```csharp
[Fact]
public async Task TestMethod()
{
    // Arrange - آماده‌سازی
    var mockRepo = new MockProductRepository();
    var handler = new CreateProductCommandHandler(mockRepo);
    var command = new CreateProductCommand { /* داده‌ها */ };

    // Act - اجرا
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert - بررسی
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
}
```

### 3. تست‌های منفی
```csharp
[Fact]
public async Task CreateProduct_ShouldFail_WhenInvalidData()
{
    // تست خطاها
    Assert.False(result.IsSuccess);
    Assert.Contains("error message", result.Message);
}
```

## عیب‌یابی

### مشکل: تست‌ها اجرا نمی‌شوند
1. بررسی کنید که `xunit` نصب باشد
2. بررسی کنید که `dotnet test` در مسیر درست اجرا شود
3. بررسی کنید که تمام dependencies موجود باشند

### مشکل: Mock کار نمی‌کند
1. بررسی کنید که interface درست پیاده‌سازی شده باشد
2. بررسی کنید که constructor درست فراخوانی شود
3. بررسی کنید که متدهای async درست استفاده شوند

### مشکل: تست‌ها fail می‌شوند
1. بررسی کنید که داده‌های mock درست باشند
2. بررسی کنید که assertion ها درست باشند
3. بررسی کنید که business logic درست پیاده‌سازی شده باشد

## اضافه کردن تست‌های جدید

### 1. تست جدید برای Command Handler
```csharp
[Fact]
public async Task NewCommand_ShouldDoSomething_WhenCondition()
{
    // Arrange
    var mockRepo = new MockProductRepository();
    var handler = new NewCommandHandler(mockRepo);
    var command = new NewCommand { /* داده‌ها */ };

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
}
```

### 2. تست جدید برای Domain Entity
```csharp
[Fact]
public void NewMethod_ShouldDoSomething_WhenCondition()
{
    // Arrange
    var product = new Product(/* پارامترها */);

    // Act
    product.NewMethod();

    // Assert
    Assert.Equal(expectedValue, product.Property);
}
```

## نتیجه‌گیری

این سیستم تست کامل برای Product شامل:
- ✅ Mock Repository کامل
- ✅ تست‌های Command Handler
- ✅ تست‌های Domain Entity
- ✅ پوشش کامل business logic
- ✅ تست‌های مثبت و منفی
- ✅ مستندات کامل

حالا می‌توانید با اطمینان کد Product را توسعه دهید! 🚀 