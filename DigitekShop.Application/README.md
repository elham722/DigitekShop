# DigitekShop Application Layer

این لایه مسئول منطق اپلیکیشن و واسط بین لایه دامین و لایه‌های خارجی است.

## 🏗️ ساختار پروژه

```
DigitekShop.Application/
├── DTOs/                    # Data Transfer Objects
│   ├── Common/             # DTO های مشترک
│   ├── Product/            # DTO های محصول
│   ├── Order/              # DTO های سفارش
│   ├── Customer/           # DTO های مشتری
│   ├── Category/           # DTO های دسته‌بندی
│   └── Brand/              # DTO های برند
├── Interfaces/             # واسط‌های CQRS
├── Profiles/               # AutoMapper Profiles
└── ApplicationServicesRegistration.cs
```

## 🎯 ویژگی‌های کلیدی

### ✅ CQRS Pattern
- **Commands**: برای عملیات تغییردهنده (Create, Update, Delete)
- **Queries**: برای عملیات خواندن (Get, List, Search)
- **Command Handlers**: پردازش دستورات
- **Query Handlers**: پردازش پرس‌وجوها

### ✅ AutoMapper Integration
- **Mapping Profiles**: نگاشت بین Entity ها و DTO ها
- **Value Object Mapping**: نگاشت Value Objects به primitive types
- **Custom Mapping**: نگاشت‌های سفارشی برای موارد پیچیده

### ✅ DTO Structure
- **BaseDto**: کلاس پایه برای تمام DTO ها
- **Command DTOs**: برای عملیات تغییردهنده
- **Query DTOs**: برای عملیات خواندن
- **Filter DTOs**: برای فیلتر کردن نتایج

## 📦 DTO ها

### Common DTOs
- **BaseDto**: کلاس پایه با Id و تاریخ‌ها
- **PagedResultDto<T>**: برای نتایج صفحه‌بندی شده
- **ApiResponseDto<T>**: برای پاسخ‌های API

### Product DTOs
- **CreateProductDto**: ایجاد محصول جدید
- **UpdateProductDto**: به‌روزرسانی محصول
- **ProductDto**: نمایش کامل محصول
- **ProductListDto**: نمایش خلاصه محصول
- **ProductFilterDto**: فیلترهای محصول

### Order DTOs
- **CreateOrderDto**: ایجاد سفارش جدید
- **OrderDto**: نمایش کامل سفارش
- **OrderItemDto**: آیتم‌های سفارش
- **AddressDto**: آدرس‌ها

### Customer DTOs
- **CustomerDto**: اطلاعات مشتری

### Category DTOs
- **CategoryDto**: اطلاعات دسته‌بندی

### Brand DTOs
- **BrandDto**: اطلاعات برند

## 🔄 AutoMapper Profiles

### Product Mappings
```csharp
// Entity به DTO
CreateMap<Product, ProductDto>()
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount));

// DTO به Entity
CreateMap<CreateProductDto, Product>()
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new ProductName(src.Name)))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Money(src.Price)));
```

### Order Mappings
```csharp
CreateMap<Order, OrderDto>()
    .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber.Value))
    .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Amount));
```

## 🎨 CQRS Interfaces

### Query Interface
```csharp
public interface IQuery<TResult>
{
}

public interface IQueryHandler<TQuery, TResult> 
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
```

### Command Interface
```csharp
public interface ICommand<TResult>
{
}

public interface ICommandHandler<TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
```

## 🚀 استفاده

### Dependency Injection
```csharp
// در Startup.cs یا Program.cs
services.ConfigureAddApplicationServices();
```

### AutoMapper Usage
```csharp
// در Service ها
public class ProductService
{
    private readonly IMapper _mapper;
    
    public ProductService(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public ProductDto GetProduct(int id)
    {
        var product = _productRepository.GetById(id);
        return _mapper.Map<ProductDto>(product);
    }
}
```

## 📋 مراحل بعدی

- [ ] پیاده‌سازی Command Handlers
- [ ] پیاده‌سازی Query Handlers
- [ ] اضافه کردن Validation
- [ ] پیاده‌سازی MediatR
- [ ] اضافه کردن Caching
- [ ] پیاده‌سازی Logging

## 🔧 نکات مهم

### Value Object Mapping
- Value Objects باید به primitive types نگاشت شوند
- از `.Value` property برای دسترسی به مقدار استفاده کنید
- برای ایجاد Value Objects از constructor استفاده کنید

### Null Safety
- از null-conditional operators استفاده کنید
- برای navigation properties null check کنید
- از default values استفاده کنید

### Performance
- از Include برای eager loading استفاده کنید
- از ProjectTo برای mapping مستقیم استفاده کنید
- از pagination برای لیست‌های بزرگ استفاده کنید

---

**نکته**: این لایه آماده برای پیاده‌سازی کامل CQRS و MediatR است. 