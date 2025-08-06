# Identity Validators

این پوشه شامل تمام Validator های مربوط به Identity است که با استفاده از FluentValidation پیاده‌سازی شده‌اند.

## ساختار

```
DigitekShop.Application/Validators/Identity/
├── CreateUserDtoValidator.cs
├── RegisterUserDtoValidator.cs
├── UpdateProfileDtoValidator.cs
├── LoginDtoValidator.cs
├── ChangePasswordDtoValidator.cs
├── ResetPasswordDtoValidator.cs
├── CreateRoleDtoValidator.cs
└── README.md
```

## Validator های موجود

### 1. CreateUserDtoValidator
- **هدف**: اعتبارسنجی ایجاد کاربر جدید توسط ادمین
- **قوانین کلیدی**:
  - ایمیل: الزامی، فرمت معتبر، حداکثر 100 کاراکتر
  - نام کاربری: الزامی، 3-50 کاراکتر، فقط حروف انگلیسی، اعداد و _
  - رمز عبور: الزامی، حداقل 6 کاراکتر، شامل حروف کوچک، بزرگ و عدد
  - نام و نام خانوادگی: الزامی، حداکثر 50 کاراکتر، فقط حروف فارسی
  - شماره تلفن: فرمت 09xxxxxxxxx
  - سن: حداقل 10 سال
  - **نکته**: تایید رمز عبور در سطح سرویس انجام می‌شود

### 2. RegisterUserDtoValidator
- **هدف**: اعتبارسنجی ثبت‌نام کاربر جدید
- **قوانین اضافی نسبت به CreateUserDto**:
  - رمز عبور: حداقل 8 کاراکتر، شامل کاراکتر خاص
  - سن: حداقل 13 سال
  - پذیرش قوانین: الزامی
  - پذیرش ایمیل‌های تبلیغاتی: الزامی
  - نقش پیش‌فرض: معتبر

### 3. UpdateProfileDtoValidator
- **هدف**: اعتبارسنجی بروزرسانی پروفایل کاربر
- **ویژگی**: Partial Update - حداقل یک فیلد باید ارائه شود
- **قوانین**: مشابه CreateUserDto اما اختیاری

### 4. LoginDtoValidator
- **هدف**: اعتبارسنجی ورود کاربر
- **قوانین**:
  - ایمیل: الزامی، فرمت معتبر
  - رمز عبور: الزامی، حداقل 1 کاراکتر
  - به خاطر سپاری: الزامی

### 5. ChangePasswordDtoValidator
- **هدف**: اعتبارسنجی تغییر رمز عبور
- **قوانین**:
  - رمز عبور فعلی: الزامی
  - رمز عبور جدید: حداقل 8 کاراکتر، شامل کاراکتر خاص
  - رمز عبور جدید نباید با فعلی یکسان باشد

### 6. ResetPasswordDtoValidator
- **هدف**: اعتبارسنجی بازنشانی رمز عبور
- **قوانین**:
  - ایمیل: الزامی، فرمت معتبر
  - توکن: الزامی، 10-500 کاراکتر
  - رمز عبور جدید: حداقل 8 کاراکتر، شامل کاراکتر خاص

### 7. CreateRoleDtoValidator
- **هدف**: اعتبارسنجی ایجاد نقش جدید
- **قوانین**:
  - نام نقش: الزامی، 2-50 کاراکتر، حروف، اعداد، _ و فاصله
  - توضیحات: اختیاری، حداکثر 200 کاراکتر

## ویژگی‌های مشترک

### پیام‌های خطا
- تمام پیام‌های خطا به زبان فارسی
- پیام‌های واضح و کاربرپسند

### Validation Rules
- **Regex Patterns**: برای اعتبارسنجی فرمت‌های خاص
- **Custom Validators**: برای قوانین پیچیده
- **Conditional Validation**: برای فیلدهای اختیاری

### امنیت
- **Password Strength**: قوانین سختگیرانه برای رمز عبور
- **Input Sanitization**: محدودیت طول و کاراکترهای مجاز
- **Age Validation**: محدودیت سن برای کاربران

## نحوه استفاده

### 1. Registration در DI Container
```csharp
// در ApplicationServicesRegistration.cs
services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
```

### 2. استفاده در Controller
```csharp
[HttpPost]
public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
{
    // Validation به صورت خودکار انجام می‌شود
    var result = await _userService.CreateUserAsync(dto);
    return Ok(result);
}
```

### 3. استفاده در Service
```csharp
public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
{
    // Validation در اینجا هم اعمال می‌شود
    var user = _mapper.Map<ApplicationUser>(dto);
    // ...
}
```

## مزایای این رویکرد

1. **یکپارچگی**: همه validation ها در Application layer
2. **قابلیت استفاده مجدد**: DTO ها در Application هستند
3. **سازگاری**: مثل Domain entities
4. **مدیریت آسان**: یک مکان برای همه validation ها
5. **امنیت**: قوانین سختگیرانه و پیام‌های فارسی
6. **قابلیت توسعه**: اضافه کردن validator جدید آسان است

## نکات مهم

- تمام validator ها از `AbstractValidator<T>` ارث‌بری می‌کنند
- پیام‌های خطا به زبان فارسی و کاربرپسند هستند
- قوانین validation بر اساس نیازهای کسب‌وکار تنظیم شده‌اند
- امکان اضافه کردن validator های جدید برای DTO های جدید وجود دارد 