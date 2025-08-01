# خدمة البريد الإلكتروني في DigitekShop

## نظرة عامة

تم تنفيذ خدمة البريد الإلكتروني في مشروع DigitekShop باستخدام نمط التصميم المعتمد على الواجهات (Interface-based design) لضمان المرونة وقابلية التوسع. يمكن استخدام مزودي خدمة بريد إلكتروني مختلفين (مثل SendGrid أو SMTP) دون الحاجة إلى تغيير الكود في طبقة التطبيق.

## الهيكل

1. **واجهة IEmailSender**: تحدد العقد الذي يجب أن تنفذه جميع خدمات البريد الإلكتروني.
2. **إعدادات البريد الإلكتروني**: فئة EmailSettings تحتوي على إعدادات التكوين اللازمة.
3. **تنفيذات مختلفة**: تم توفير تنفيذين مختلفين (SendGrid و SMTP) لإظهار المرونة.

## التكوين

### 1. إعدادات البريد الإلكتروني

يجب إضافة إعدادات البريد الإلكتروني في ملف `appsettings.json`:

```json
"EmailSettings": {
  "ApiKey": "YOUR_SENDGRID_API_KEY",
  "FromAddress": "noreply@digitekshop.com",
  "FromName": "DigitekShop",
  "SmtpServer": "smtp.sendgrid.net",
  "SmtpPort": 587,
  "SmtpUsername": "apikey",
  "SmtpPassword": "YOUR_SENDGRID_API_KEY",
  "EnableSsl": true
}
```

### 2. تسجيل الخدمة

تم تسجيل خدمة البريد الإلكتروني في `InfrastructureServiceRegistration.cs`:

```csharp
// Register Email Settings from configuration
services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

// Register Email Service - Choose one implementation based on your needs
// For SendGrid
services.AddTransient<IEmailSender, SendGridEmailSender>();

// For SMTP (uncomment if you prefer SMTP over SendGrid)
// services.AddTransient<IEmailSender, SmtpEmailSender>();
```

## استخدام خدمة البريد الإلكتروني

### 1. حقن الخدمة (Dependency Injection)

```csharp
private readonly IEmailSender _emailSender;

public YourClass(IEmailSender emailSender)
{
    _emailSender = emailSender;
}
```

### 2. إرسال بريد إلكتروني بسيط

```csharp
await _emailSender.SendEmailAsync(
    "recipient@example.com",
    "Subject Line",
    "Email body content",
    isHtml: false);
```

### 3. إرسال بريد إلكتروني HTML

```csharp
string htmlBody = "<h1>Hello World</h1><p>This is an HTML email.</p>";

await _emailSender.SendEmailAsync(
    "recipient@example.com",
    "HTML Email Subject",
    htmlBody,
    isHtml: true);
```

### 4. إرسال بريد إلكتروني مع مرفقات

```csharp
List<string> attachments = new List<string>
{
    "path/to/file1.pdf",
    "path/to/file2.jpg"
};

await _emailSender.SendEmailWithAttachmentsAsync(
    "recipient@example.com",
    "Email with Attachments",
    "Please find the attached files.",
    attachments,
    isHtml: false);
```

## أمثلة تنفيذ

1. **إرسال بريد ترحيب عند تسجيل العميل**: انظر `RegisterCustomerCommandHandler.cs`
2. **إرسال تأكيد الطلب**: انظر `OrderConfirmationEmailService.cs`

## تبديل مزودي خدمة البريد الإلكتروني

لتبديل مزود خدمة البريد الإلكتروني، قم بتعديل تسجيل الخدمة في `InfrastructureServiceRegistration.cs`:

```csharp
// للتبديل إلى SMTP
services.AddTransient<IEmailSender, SmtpEmailSender>();

// للتبديل إلى SendGrid
// services.AddTransient<IEmailSender, SendGridEmailSender>();
```

## إضافة مزود خدمة بريد إلكتروني جديد

1. قم بإنشاء فئة جديدة تنفذ واجهة `IEmailSender`
2. قم بتنفيذ الطرق المطلوبة
3. قم بتسجيل التنفيذ الجديد في `InfrastructureServiceRegistration.cs`

## ملاحظات هامة

1. **أمان**: لا تقم أبدًا بتخزين مفاتيح API أو كلمات المرور في التعليمات البرمجية. استخدم دائمًا ملفات التكوين وأسرار المستخدم.
2. **معالجة الأخطاء**: تأكد من معالجة الأخطاء بشكل صحيح عند استخدام خدمة البريد الإلكتروني.
3. **الاختبار**: قم بإنشاء تنفيذ وهمي (Mock) لـ `IEmailSender` لاستخدامه في الاختبارات.