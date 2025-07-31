# راهنمای اضافه کردن پروژه به GitHub

## 📋 مراحل اضافه کردن پروژه به GitHub

### ۱. ایجاد Repository در GitHub
1. به [GitHub.com](https://github.com) بروید
2. روی دکمه **"New"** کلیک کنید
3. نام repository را **"DigitekShop"** قرار دهید
4. توضیحات: **"فروشگاه آنلاین لوازم الکترونیک با Clean Architecture"**
5. Repository را **Public** یا **Private** انتخاب کنید
6. روی **"Create repository"** کلیک کنید

### ۲. تنظیم Git در پروژه محلی

#### الف) تنظیم نام و ایمیل:
```bash
git config --global user.name "نام شما"
git config --global user.email "ایمیل شما"
```

#### ب) اضافه کردن فایل‌ها:
```bash
git add .
git commit -m "Initial commit: Clean Architecture Domain Layer"
```

#### ج) اتصال به GitHub:
```bash
git remote add origin https://github.com/yourusername/DigitekShop.git
git branch -M main
git push -u origin main
```

### ۳. محتوای پروژه

پروژه شامل موارد زیر است:

#### ✅ Domain Layer (کامل)
- **9 Entity**: Product, Category, Customer, Order, OrderItem, Brand, Review, Wishlist, ProductSpecification
- **7 Value Object**: ProductName, Money, SKU, Email, PhoneNumber, Address, OrderNumber
- **5 Enum**: ProductStatus, CategoryType, CustomerStatus, OrderStatus, PaymentMethod
- **8 Repository Interface**: IGenericRepository + 7 Specific Repositories
- **3 Domain Event**: ProductCreatedEvent, OrderCreatedEvent, IDomainEvent
- **2 Exception**: DomainException, ProductNotFoundException

#### 📁 فایل‌های اضافی
- **.gitignore**: برای .NET projects
- **README.md**: مستندات کامل پروژه
- **DigitekShop.sln**: فایل solution

### ۴. ساختار نهایی در GitHub

```
DigitekShop/
├── .gitignore
├── README.md
├── GITHUB_SETUP.md
├── DigitekShop.sln
└── DigitekShop.Domain/
    ├── Entities/
    │   ├── Common/
    │   │   └── BaseEntity.cs
    │   ├── Product.cs
    │   ├── Category.cs
    │   ├── Customer.cs
    │   ├── Brand.cs
    │   ├── ProductSpecification.cs
    │   ├── Order.cs
    │   ├── OrderItem.cs
    │   ├── Review.cs
    │   └── Wishlist.cs
    ├── ValueObjects/
    │   ├── ProductName.cs
    │   ├── Money.cs
    │   ├── SKU.cs
    │   ├── Email.cs
    │   ├── PhoneNumber.cs
    │   ├── Address.cs
    │   └── OrderNumber.cs
    ├── Enums/
    │   ├── ProductStatus.cs
    │   ├── CategoryType.cs
    │   ├── CustomerStatus.cs
    │   ├── OrderStatus.cs
    │   └── PaymentMethod.cs
    ├── Interfaces/
    │   ├── IGenericRepository.cs
    │   ├── IProductRepository.cs
    │   ├── ICategoryRepository.cs
    │   ├── ICustomerRepository.cs
    │   ├── IBrandRepository.cs
    │   ├── IOrderRepository.cs
    │   ├── IReviewRepository.cs
    │   └── IWishlistRepository.cs
    ├── Events/
    │   ├── IDomainEvent.cs
    │   ├── ProductCreatedEvent.cs
    │   └── OrderCreatedEvent.cs
    └── Exceptions/
        ├── DomainException.cs
        └── ProductNotFoundException.cs
```

### ۵. نکات مهم

#### 🔒 امنیت
- فایل‌های حساس مثل connection string ها را در .gitignore قرار دهید
- از environment variables برای تنظیمات استفاده کنید

#### 📝 Commit Messages
- از commit message های معنادار استفاده کنید
- از prefix های مناسب استفاده کنید:
  - `feat:` برای ویژگی‌های جدید
  - `fix:` برای رفع باگ
  - `docs:` برای مستندات
  - `refactor:` برای بازنویسی کد

#### 🌿 Branching Strategy
- از **main** برای کد پایدار استفاده کنید
- از **feature branches** برای توسعه ویژگی‌های جدید
- از **Pull Request** برای merge کردن استفاده کنید

### ۶. مراحل بعدی

بعد از اضافه کردن به GitHub:

1. **Infrastructure Layer** را پیاده‌سازی کنید
2. **Application Layer** را اضافه کنید
3. **API Layer** را ایجاد کنید
4. **UI Layer** را پیاده‌سازی کنید
5. **Tests** را اضافه کنید

---

**نکته**: این راهنما برای اضافه کردن پروژه به GitHub است. بعد از ایجاد repository در GitHub، دستورات بالا را اجرا کنید. 