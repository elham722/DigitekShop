# اختبارات DigitekShop

## نظرة عامة

يحتوي هذا المجلد على اختبارات وحدة (Unit Tests) واختبارات تكامل (Integration Tests) لمشروع DigitekShop. تم تصميم الاختبارات باستخدام مبدأ الاعتماد على الواجهات (Interfaces) بدلاً من التنفيذات المحددة (Concrete Implementations)، مما يسمح باختبار كل طبقة بشكل معزول.

## هيكل المجلد

- **Application**: اختبارات لطبقة التطبيق
  - **Features**: اختبارات لميزات التطبيق المختلفة
    - **Customers**: اختبارات لميزات العملاء
    - **Orders**: اختبارات لميزات الطلبات
    - **Products**: اختبارات لميزات المنتجات
- **Domain**: اختبارات لطبقة المجال
  - **Entities**: اختبارات للكيانات
  - **ValueObjects**: اختبارات لكائنات القيمة
- **Infrastructure**: اختبارات لطبقة البنية التحتية
  - **Email**: اختبارات لخدمات البريد الإلكتروني
- **Mocks**: تنفيذات وهمية للواجهات المستخدمة في الاختبارات

## استخدام التنفيذات الوهمية (Mocks)

تستخدم الاختبارات تنفيذات وهمية للواجهات لعزل الوحدة المختبرة عن التبعيات الخارجية. هناك طريقتان رئيسيتان لإنشاء التنفيذات الوهمية:

### 1. استخدام مكتبة Moq

تُستخدم مكتبة Moq لإنشاء تنفيذات وهمية للواجهات بشكل ديناميكي:

```csharp
// إنشاء تنفيذ وهمي لواجهة IEmailSender
var mockEmailSender = new Mock<IEmailSender>();

// تهيئة السلوك المتوقع
mockEmailSender.Setup(sender => sender.SendEmailAsync(
    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
    .ReturnsAsync(true);

// استخدام التنفيذ الوهمي
var emailSender = mockEmailSender.Object;

// التحقق من استدعاء الطريقة
mockEmailSender.Verify(sender => sender.SendEmailAsync(
    "test@example.com", "Test Subject", "Test Body", true), Times.Once);
```

### 2. استخدام تنفيذات وهمية مخصصة

تم إنشاء تنفيذات وهمية مخصصة للواجهات الأكثر استخدامًا في المشروع:

- **MockEmailSender**: تنفيذ وهمي لواجهة IEmailSender
- **MockEmailTemplateService**: تنفيذ وهمي لواجهة IEmailTemplateService

مثال على استخدام MockEmailTemplateService:

```csharp
// إنشاء تنفيذ وهمي مخصص
var mockTemplateService = new MockEmailTemplateService();

// استخدام التنفيذ الوهمي
string template = await mockTemplateService.GetProcessedTemplateAsync("WelcomeEmail", new Dictionary<string, string>
{
    { "FirstName", "John" }
});

// التحقق من النتيجة
Assert.Contains("Welcome John", template);
```

## أفضل الممارسات

1. **استخدام الواجهات**: اعتمد دائمًا على الواجهات بدلاً من التنفيذات المحددة في الاختبارات.
2. **عزل الوحدة المختبرة**: تأكد من اختبار الوحدة بشكل معزول عن التبعيات الخارجية.
3. **تنظيم الاختبارات**: نظم الاختبارات بنفس هيكل الكود المختبر.
4. **تسمية الاختبارات**: استخدم تسمية واضحة للاختبارات تصف السلوك المتوقع.
5. **استخدام AAA**: اتبع نمط Arrange-Act-Assert في كتابة الاختبارات.

## إضافة اختبارات جديدة

عند إضافة اختبارات جديدة، اتبع الخطوات التالية:

1. أنشئ ملف اختبار جديد في المجلد المناسب.
2. استخدم التنفيذات الوهمية المناسبة لعزل الوحدة المختبرة.
3. اتبع نمط AAA (Arrange-Act-Assert) في كتابة الاختبارات.
4. تأكد من تغطية جميع حالات الاستخدام المتوقعة.

## تشغيل الاختبارات

يمكن تشغيل الاختبارات باستخدام الأمر التالي:

```bash
dotnet test
```

أو باستخدام Visual Studio Test Explorer.