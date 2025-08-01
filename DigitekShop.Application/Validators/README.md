# FluentValidation Implementation - پیاده‌سازی اعتبارسنجی

## 📋 **Overview**

این پروژه از **FluentValidation** برای اعتبارسنجی Command ها و Query ها استفاده می‌کند. این کتابخانه امکان تعریف قوانین اعتبارسنجی پیچیده و قابل تست را فراهم می‌کند.

## 🏗️ **ساختار پروژه**

```
DigitekShop.Application/
  Validators/
    Products/
      CreateProductCommandValidator.cs
      UpdateProductCommandValidator.cs
      DeleteProductCommandValidator.cs
      GetProductsQueryValidator.cs
    Customers/
      CreateCustomerCommandValidator.cs
    Orders/
      CreateOrderCommandValidator.cs
  Services/
    ValidationService.cs
  Exceptions/
    ValidationException.cs
```

## 🔧 **ویژگی‌های پیاده‌سازی شده**

### **1. اعتبارسنجی محصولات (Products)**

#### **CreateProductCommandValidator:**
- نام محصول: الزامی، حداکثر 100 کاراکتر
- توضیحات: الزامی، حداکثر 1000 کاراکتر
- قیمت: بیشتر از صفر، حداکثر 1 میلیارد تومان
- موجودی: غیر منفی، حداکثر 100,000
- SKU: الزامی، حداکثر 50 کاراکتر، فقط حروف بزرگ و اعداد
- دسته‌بندی: الزامی
- برند: اختیاری اما معتبر
- وزن: اختیاری، 0 تا 1000 کیلوگرم
- تصویر: اختیاری، URL معتبر

#### **UpdateProductCommandValidator:**
- مشابه CreateProduct + شناسه محصول الزامی

#### **DeleteProductCommandValidator:**
- شناسه محصول الزامی

#### **GetProductsQueryValidator:**
- شماره صفحه: 1 تا 10000
- اندازه صفحه: 1 تا 100
- جستجو: حداکثر 100 کاراکتر
- قیمت: محدوده معتبر
- مرتب‌سازی: فیلدهای مجاز

### **2. اعتبارسنجی مشتریان (Customers)**

#### **CreateCustomerCommandValidator:**
- نام و نام خانوادگی: الزامی، حداکثر 50 کاراکتر، فقط فارسی
- ایمیل: الزامی، فرمت معتبر
- تلفن: الزامی، فرمت 09xxxxxxxxx
- کد ملی: الزامی، 10 رقم
- تاریخ تولد: الزامی، سن 10 تا 120 سال
- آدرس: اختیاری، محدودیت طول
- کد پستی: 10 رقم
- تصویر پروفایل: اختیاری، URL معتبر

### **3. اعتبارسنجی سفارشات (Orders)**

#### **CreateOrderCommandValidator:**
- شناسه مشتری: الزامی
- آیتم‌های سفارش: حداقل یک آیتم
- هر آیتم: شناسه محصول، تعداد، قیمت واحد
- روش پرداخت و ارسال: معتبر
- آدرس ارسال و صورتحساب: الزامی
- یادداشت: اختیاری، حداکثر 1000 کاراکتر

## 🚀 **نحوه استفاده**

### **1. در Handler:**

```csharp
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
{
    private readonly IValidationService _validationService;

    public async Task<ProductDto> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        // اعتبارسنجی Command
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // ادامه منطق...
    }
}
```

### **2. در Controller:**

```csharp
[HttpPost]
public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
{
    try
    {
        var command = _mapper.Map<CreateProductCommand>(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    catch (ValidationException ex)
    {
        return BadRequest(new { Errors = ex.Errors });
    }
}
```

## 📝 **اضافه کردن Validator جدید**

### **1. ایجاد Validator:**

```csharp
public class NewCommandValidator : AbstractValidator<NewCommand>
{
    public NewCommandValidator()
    {
        RuleFor(x => x.Property)
            .NotEmpty().WithMessage("پیام خطا");
    }
}
```

### **2. ثبت در DI:**

Validator ها به صورت خودکار در `FeaturesRegistration` ثبت می‌شوند:

```csharp
services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
```

## 🔍 **قوانین اعتبارسنجی پیشرفته**

### **1. اعتبارسنجی شرطی:**

```csharp
RuleFor(x => x.BrandId)
    .GreaterThan(0).When(x => x.BrandId.HasValue)
    .WithMessage("شناسه برند باید معتبر باشد");
```

### **2. اعتبارسنجی سفارشی:**

```csharp
RuleFor(x => x.SKU)
    .Must(BeValidSku).WithMessage("SKU معتبر نیست");

private bool BeValidSku(string sku)
{
    return !string.IsNullOrEmpty(sku) && sku.Length >= 5;
}
```

### **3. اعتبارسنجی مجموعه:**

```csharp
RuleForEach(x => x.OrderItems).ChildRules(item =>
{
    item.RuleFor(x => x.Quantity)
        .GreaterThan(0).WithMessage("تعداد باید بیشتر از صفر باشد");
});
```

## 🎯 **مزایای این پیاده‌سازی**

1. **جداسازی منطق:** اعتبارسنجی از Handler جدا شده
2. **قابلیت تست:** هر Validator قابل تست مستقل است
3. **پیام‌های فارسی:** تمام پیام‌های خطا به فارسی
4. **انعطاف‌پذیری:** قوانین پیچیده و شرطی
5. **یکپارچگی:** با معماری Clean Architecture سازگار
6. **قابلیت توسعه:** اضافه کردن Validator جدید آسان

## 📚 **مراجع**

- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [Validation Best Practices](https://docs.fluentvalidation.net/en/latest/best-practices.html) 