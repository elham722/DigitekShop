# DigitekShop Domain Layer

این لایه شامل تمام منطق کسب و کار و قوانین دامنه برای فروشگاه آنلاین لوازم الکترونیک است.

## ساختار پروژه

### Entities
- **Product**: محصولات فروشگاه
- **Order**: سفارشات مشتریان
- **OrderItem**: آیتم‌های هر سفارش
- **Customer**: مشتریان
- **Category**: دسته‌بندی محصولات
- **Brand**: برندها
- **Review**: نظرات محصولات
- **Wishlist**: لیست علاقه‌مندی‌ها

### Value Objects
- **Money**: برای مدیریت پول و قیمت‌ها
- **Email**: ایمیل با validation
- **PhoneNumber**: شماره تلفن با فرمت‌های مختلف
- **Address**: آدرس کامل
- **ProductName**: نام محصول
- **SKU**: کد محصول
- **OrderNumber**: شماره سفارش

### Enums
- **OrderStatus**: وضعیت‌های مختلف سفارش
- **ProductStatus**: وضعیت‌های مختلف محصول
- **CustomerStatus**: وضعیت‌های مختلف مشتری
- **PaymentMethod**: روش‌های پرداخت
- **CategoryType**: انواع دسته‌بندی

### Domain Services
- **OrderDomainService**: سرویس‌های مربوط به سفارش
- **ProductDomainService**: سرویس‌های مربوط به محصول
- **DiscountCalculatorService**: محاسبه تخفیف‌ها

### Specifications
- **ProductSpecifications**: فیلترهای مختلف برای محصولات
- **OrderSpecifications**: فیلترهای مختلف برای سفارشات

### Business Rules
- **OrderBusinessRules**: قوانین کسب و کار برای سفارشات
- **BusinessRuleValidator**: اعتبارسنجی قوانین

### Policies
- **DiscountPolicies**: سیاست‌های مختلف تخفیف
- **IDiscountPolicy**: interface برای سیاست‌های تخفیف

### Events
- **OrderCreatedEvent**: رویداد ایجاد سفارش
- **ProductCreatedEvent**: رویداد ایجاد محصول
- **OrderStatusChangedEvent**: رویداد تغییر وضعیت سفارش
- **ProductStockUpdatedEvent**: رویداد تغییر موجودی
- **CustomerRegisteredEvent**: رویداد ثبت‌نام مشتری

### Exceptions
- **DomainException**: خطای عمومی دامنه
- **ProductNotFoundException**: محصول یافت نشد
- **OrderNotFoundException**: سفارش یافت نشد
- **CustomerNotFoundException**: مشتری یافت نشد
- **InsufficientStockException**: موجودی ناکافی
- **InvalidOrderStatusException**: وضعیت نامعتبر سفارش

### Aggregates
- **OrderAggregate**: Aggregate Root برای سفارش

## ویژگی‌های کلیدی

### 1. Encapsulation
تمام entities دارای encapsulation مناسب هستند و فقط از طریق business methods قابل تغییر هستند.

### 2. Business Logic
تمام منطق کسب و کار در domain layer قرار دارد و قابل تست است.

### 3. Value Objects
استفاده از Value Objects برای جلوگیری از primitive obsession و افزایش type safety.

### 4. Domain Events
استفاده از Domain Events برای loose coupling بین بخش‌های مختلف سیستم.

### 5. Specifications
استفاده از Specification pattern برای query های پیچیده.

### 6. Business Rules
استفاده از Business Rules برای اعتبارسنجی قوانین کسب و کار.

### 7. Policies
استفاده از Policy pattern برای قوانین قابل تغییر مانند تخفیف‌ها.

## مثال‌های استفاده

### ایجاد سفارش
```csharp
var orderAggregate = OrderAggregate.Create(
    customerId: 1,
    shippingAddress: new Address("تهران", "تهران", "شمال", "ولیعصر"),
    billingAddress: new Address("تهران", "تهران", "شمال", "ولیعصر"),
    paymentMethod: PaymentMethod.Online
);

orderAggregate.AddOrderItem(product, 2, product.Price);
```

### اعمال تخفیف
```csharp
var discountService = new DiscountCalculatorService();
discountService.AddPolicy(new ExpensiveProductDiscountPolicy(1000000m, 10));
var discount = discountService.CalculateBestDiscount(product);
```

### اعتبارسنجی قوانین
```csharp
var validator = new BusinessRuleValidator();
validator.AddRule(new OrderMustHaveItemsRule(order.OrderItems));
validator.AddRule(new CustomerMustBeActiveRule(customer));
validator.Validate();
```

## نکات مهم

1. **Immutability**: Value Objects immutable هستند
2. **Validation**: تمام validation ها در constructor ها انجام می‌شود
3. **Business Methods**: تمام تغییرات از طریق business methods انجام می‌شود
4. **Domain Events**: برای هر تغییر مهم domain event تولید می‌شود
5. **Specifications**: برای query های پیچیده از specifications استفاده می‌شود
6. **Policies**: برای قوانین قابل تغییر از policies استفاده می‌شود

## تست‌پذیری

تمام کامپوننت‌ها قابل تست هستند و dependency injection استفاده شده است. 