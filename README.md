# DigitekShop - فروشگاه آنلاین لوازم الکترونیک

## 🏗️ معماری پروژه

این پروژه با استفاده از **Clean Architecture** و **Domain-Driven Design (DDD)** پیاده‌سازی شده است.

### 📁 ساختار پروژه

```
DigitekShop/
├── src/
│   ├── Core/
│   │   ├── DigitekShop.Domain/          # لایه دامین
│   │   └── DigitekShop.Application/     # لایه اپلیکیشن
│   ├── Infrastructure/                  # لایه زیرساخت
│   ├── API/                             # لایه API
│   └── UI/                              # لایه رابط کاربری
└── tests/                               # تست‌ها
```

## 🎯 لایه دامین (Domain Layer)

### 📦 موجودیت‌ها (Entities)
- **Product**: محصولات فروشگاه
- **Category**: دسته‌بندی محصولات
- **Customer**: مشتریان
- **Order**: سفارشات
- **OrderItem**: آیتم‌های سفارش
- **Brand**: برندها
- **Review**: نظرات محصولات
- **Wishlist**: لیست علاقه‌مندی‌ها
- **ProductSpecification**: مشخصات فنی محصولات

### 💎 Value Objects
- **ProductName**: نام محصول
- **Money**: مقادیر پولی
- **SKU**: کد محصول
- **Email**: ایمیل
- **PhoneNumber**: شماره تلفن
- **Address**: آدرس
- **OrderNumber**: شماره سفارش

### 🔄 Repository Pattern
- **IGenericRepository<T>**: Repository عمومی برای عملیات CRUD
- **IProductRepository**: Repository خاص محصولات
- **ICategoryRepository**: Repository خاص دسته‌بندی‌ها
- **ICustomerRepository**: Repository خاص مشتریان
- **IOrderRepository**: Repository خاص سفارشات
- **IBrandRepository**: Repository خاص برندها
- **IReviewRepository**: Repository خاص نظرات
- **IWishlistRepository**: Repository خاص لیست علاقه‌مندی‌ها

### 🎭 Enums
- **ProductStatus**: وضعیت محصول
- **CategoryType**: نوع دسته‌بندی
- **CustomerStatus**: وضعیت مشتری
- **OrderStatus**: وضعیت سفارش
- **PaymentMethod**: روش پرداخت

## 🚀 ویژگی‌های کلیدی

### ✅ Clean Architecture
- **جداسازی لایه‌ها**: هر لایه مسئولیت مشخصی دارد
- **وابستگی یکطرفه**: لایه‌های داخلی به لایه‌های خارجی وابسته نیستند
- **قابلیت تست**: هر لایه به صورت مستقل قابل تست است

### ✅ Domain-Driven Design
- **Rich Domain Model**: موجودیت‌ها دارای رفتار هستند
- **Value Objects**: برای مقادیر غیرقابل تغییر
- **Business Methods**: متدهای تجاری در موجودیت‌ها
- **Encapsulation**: محافظت از داده‌ها

### ✅ Repository Pattern
- **Generic Repository**: عملیات CRUD عمومی
- **Specific Repository**: عملیات خاص هر موجودیت
- **Abstraction**: جداسازی منطق دسترسی به داده

## 🛠️ تکنولوژی‌های استفاده شده

- **.NET 8**
- **C# 12**
- **Entity Framework Core**
- **Clean Architecture**
- **Domain-Driven Design**

## 📋 مراحل بعدی

- [ ] پیاده‌سازی Infrastructure Layer
- [ ] پیاده‌سازی Application Layer
- [ ] پیاده‌سازی API Layer
- [ ] پیاده‌سازی UI Layer
- [ ] اضافه کردن تست‌ها

## 👨‍💻 توسعه‌دهنده

این پروژه با راهنمایی و پیاده‌سازی Clean Architecture و DDD ایجاد شده است.

---

**نکته**: این پروژه در حال توسعه است و ممکن است تغییراتی در ساختار آن اعمال شود. 