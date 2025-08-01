# Bug Fixes - برطرف کردن خطاهای کامپایل

## 🐛 **خطاهای شناسایی شده:**

### 1. **خطای Value Objects**
```
Argument 1: cannot convert from 'string' to 'DigitekShop.Domain.ValueObjects.PhoneNumber'
Argument 1: cannot convert from 'string' to 'DigitekShop.Domain.ValueObjects.Email'
```

**علت:** Repository methods انتظار Value Objects دارند، نه string

**راه‌حل:**
```csharp
// قبل (خطا):
var existingCustomerByEmail = await _unitOfWork.Customers.GetByEmailAsync(command.Email);

// بعد (صحیح):
var existingCustomerByEmail = await _unitOfWork.Customers.GetByEmailAsync(new Email(command.Email));
```

### 2. **خطای Mapping Profile**
```
'CustomerMappingProfile' does not contain a definition for 'MapToDto'
```

**علت:** استفاده از `CustomerMappingProfile` به جای `IMapper`

**راه‌حل:**
```csharp
// قبل (خطا):
private readonly CustomerMappingProfile _mapper;
return _mapper.MapToDto(createdCustomer);

// بعد (صحیح):
private readonly IMapper _mapper;
return _mapper.Map<CustomerDto>(createdCustomer);
```

## ✅ **تغییرات انجام شده:**

### 1. **به‌روزرسانی Constructor Dependencies:**
```csharp
// قبل:
public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, CustomerMappingProfile mapper)

// بعد:
public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
```

### 2. **به‌روزرسانی Value Object Calls:**
```csharp
// قبل:
await _unitOfWork.Customers.GetByEmailAsync(command.Email);
await _unitOfWork.Customers.GetByPhoneAsync(command.Phone);
await _unitOfWork.Products.GetBySKUAsync(command.SKU);

// بعد:
await _unitOfWork.Customers.GetByEmailAsync(new Email(command.Email));
await _unitOfWork.Customers.GetByPhoneAsync(new PhoneNumber(command.Phone));
await _unitOfWork.Products.GetBySKUAsync(new SKU(command.SKU));
```

### 3. **به‌روزرسانی Mapping Calls:**
```csharp
// قبل:
return _mapper.MapToDto(createdCustomer);
return _mapper.MapToListDto(product);

// بعد:
return _mapper.Map<CustomerDto>(createdCustomer);
return _mapper.Map<ProductListDto>(product);
```

## 📋 **Handlers به‌روزرسانی شده:**

1. ✅ `CreateCustomerCommandHandler`
2. ✅ `CreateProductCommandHandler`
3. ✅ `UpdateProductCommandHandler`
4. ✅ `DeleteProductCommandHandler`
5. ✅ `GetProductQueryHandler`
6. ✅ `GetProductsQueryHandler`
7. ✅ `CreateOrderCommandHandler`

## 🔧 **نکات مهم:**

### **Value Objects:**
- Repository methods انتظار Value Objects دارند
- باید از constructor های Value Objects استفاده کنید
- مثال: `new Email(command.Email)`, `new PhoneNumber(command.Phone)`

### **AutoMapper:**
- از `IMapper` استفاده کنید، نه از Profile classes
- از `Map<T>()` استفاده کنید، نه از `MapToDto()`
- مثال: `_mapper.Map<CustomerDto>(customer)`

### **Dependency Injection:**
- `IMapper` در DI Container ثبت شده است
- نیازی به ثبت جداگانه Profile classes نیست

## 🚀 **مراحل بعدی:**

1. **تست کامپایل**: اطمینان از عدم وجود خطای کامپایل
2. **پیاده‌سازی Infrastructure**: UnitOfWork و Repository ها
3. **تست عملکرد**: تست عملیات‌های مختلف
4. **بهینه‌سازی**: بهبود performance و error handling 