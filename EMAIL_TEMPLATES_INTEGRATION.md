# دليل تكامل قوالب البريد الإلكتروني في مشروع DigitekShop

## مقدمة

تم تحسين خدمة البريد الإلكتروني في مشروع DigitekShop بإضافة نظام قوالب البريد الإلكتروني. يسمح هذا النظام بفصل محتوى البريد الإلكتروني عن الكود، مما يجعل من السهل تحديث وتخصيص رسائل البريد الإلكتروني دون الحاجة إلى تغيير الكود.

## الهيكل العام

### المكونات الرئيسية

1. **EmailTemplateService**: خدمة لتحميل ومعالجة قوالب البريد الإلكتروني
2. **قوالب HTML**: ملفات HTML تحتوي على عناصر نائبة (placeholders)
3. **تكامل مع IEmailSender**: استخدام القوالب مع خدمة إرسال البريد الإلكتروني

## التكوين والإعداد

### 1. تسجيل الخدمات

تم تحديث `InfrastructureServiceRegistration.cs` لتسجيل خدمة قوالب البريد الإلكتروني:

```csharp
services.AddSingleton<EmailTemplateService>(provider => 
{
    string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
    return new EmailTemplateService(basePath);
});
```

### 2. نسخ القوالب عند النشر

تم إنشاء ملف `CopyEmailTemplates.targets` لضمان نسخ قوالب البريد الإلكتروني إلى مجلد الإخراج أثناء عملية النشر. يجب تضمين هذا الملف في مشروع الويب عند إنشائه.

## القوالب المتوفرة

### 1. قالب الترحيب (WelcomeEmail.html)

يستخدم لإرسال رسائل ترحيب للعملاء الجدد. العناصر النائبة المتوفرة:

- `{{FirstName}}`: اسم العميل الأول
- `{{Email}}`: البريد الإلكتروني للعميل
- `{{CurrentYear}}`: السنة الحالية
- `{{CompanyName}}`: اسم الشركة
- `{{StoreUrl}}`: رابط المتجر
- `{{PrivacyPolicyUrl}}`: رابط سياسة الخصوصية
- `{{TermsOfServiceUrl}}`: رابط شروط الخدمة

### 2. قالب تأكيد الطلب (OrderConfirmation.html)

يستخدم لإرسال تأكيدات الطلبات. العناصر النائبة المتوفرة:

- `{{OrderNumber}}`: رقم الطلب
- `{{CustomerName}}`: اسم العميل الكامل
- `{{OrderDate}}`: تاريخ الطلب
- `{{PaymentMethod}}`: طريقة الدفع
- `{{OrderItems}}`: جدول HTML يحتوي على عناصر الطلب
- `{{Subtotal}}`: المجموع الفرعي
- `{{ShippingCost}}`: تكلفة الشحن
- `{{TaxAmount}}`: مبلغ الضريبة
- `{{DiscountAmount}}`: مبلغ الخصم
- `{{TotalAmount}}`: المبلغ الإجمالي
- `{{ShippingStreet}}`: عنوان الشارع للشحن
- `{{ShippingCity}}`: مدينة الشحن
- `{{ShippingState}}`: ولاية الشحن
- `{{ShippingZipCode}}`: الرمز البريدي للشحن
- `{{ShippingCountry}}`: بلد الشحن
- `{{OrderTrackingUrl}}`: رابط تتبع الطلب

## كيفية الاستخدام

### مثال: إرسال بريد إلكتروني ترحيبي

```csharp
public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Guid>
{
    private readonly IEmailSender _emailSender;
    private readonly EmailTemplateService _emailTemplateService;

    public RegisterCustomerCommandHandler(IEmailSender emailSender, EmailTemplateService emailTemplateService)
    {
        _emailSender = emailSender;
        _emailTemplateService = emailTemplateService;
    }

    private async Task SendWelcomeEmailAsync(Customer customer)
    {
        string subject = "Welcome to DigitekShop!";
        
        var replacements = new Dictionary<string, string>
        {
            { "FirstName", customer.FirstName },
            { "Email", customer.Email.Value },
            { "CurrentYear", DateTime.Now.Year.ToString() },
            { "CompanyName", "DigitekShop" },
            { "StoreUrl", "https://digitekshop.com" },
            { "PrivacyPolicyUrl", "https://digitekshop.com/privacy-policy" },
            { "TermsOfServiceUrl", "https://digitekshop.com/terms-of-service" }
        };
        
        string body = await _emailTemplateService.GetProcessedTemplateAsync("WelcomeEmail", replacements);
        await _emailSender.SendEmailAsync(customer.Email.Value, subject, body, true);
    }
}
```

### مثال: إرسال بريد إلكتروني تأكيد الطلب

```csharp
public class OrderConfirmationEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly EmailTemplateService _emailTemplateService;

    public OrderConfirmationEmailService(IEmailSender emailSender, EmailTemplateService emailTemplateService)
    {
        _emailSender = emailSender;
        _emailTemplateService = emailTemplateService;
    }

    public async Task SendOrderConfirmationEmailAsync(Order order, Customer customer)
    {
        string subject = $"DigitekShop Order Confirmation - Order #{order.OrderNumber.Value}";
        
        // إعداد العناصر النائبة
        var replacements = new Dictionary<string, string>
        {
            { "OrderNumber", order.OrderNumber.Value },
            { "CustomerName", $"{customer.FirstName} {customer.LastName}" },
            { "OrderDate", order.OrderDate.ToString("MMMM dd, yyyy") },
            { "PaymentMethod", order.PaymentMethod.ToString() },
            { "Subtotal", $"${order.Subtotal.Amount:F2}" },
            { "ShippingCost", $"${order.ShippingCost.Amount:F2}" },
            { "TaxAmount", $"${order.TaxAmount.Amount:F2}" },
            { "DiscountAmount", $"${order.DiscountAmount.Amount:F2}" },
            { "TotalAmount", $"${order.TotalAmount.Amount:F2}" },
            { "ShippingStreet", order.ShippingAddress.Street },
            { "ShippingCity", order.ShippingAddress.City },
            { "ShippingState", order.ShippingAddress.State },
            { "ShippingZipCode", order.ShippingAddress.ZipCode },
            { "ShippingCountry", order.ShippingAddress.Country },
            { "OrderTrackingUrl", $"https://digitekshop.com/orders/{order.Id}/track" }
        };
        
        // إنشاء HTML لعناصر الطلب
        var orderItemsHtml = new StringBuilder();
        foreach (var item in order.OrderItems)
        {
            orderItemsHtml.AppendLine("<tr>");
            orderItemsHtml.AppendLine($"<td>{item.ProductName}</td>");
            orderItemsHtml.AppendLine($"<td>{item.Quantity}</td>");
            orderItemsHtml.AppendLine($"<td>${item.UnitPrice.Amount:F2}</td>");
            orderItemsHtml.AppendLine($"<td>${(item.UnitPrice.Amount * item.Quantity):F2}</td>");
            orderItemsHtml.AppendLine("</tr>");
        }
        replacements.Add("OrderItems", orderItemsHtml.ToString());
        
        // الحصول على القالب المعالج
        string body = await _emailTemplateService.GetProcessedTemplateAsync("OrderConfirmation", replacements);

        // إرسال البريد الإلكتروني
        await _emailSender.SendEmailAsync(customer.Email.Value, subject, body, true);
    }
}
```

## إنشاء قوالب جديدة

### الخطوات

1. قم بإنشاء ملف HTML جديد في مجلد `EmailTemplates`
2. استخدم الصيغة `{{PlaceholderName}}` للعناصر النائبة
3. قم بتصميم القالب باستخدام HTML و CSS المضمن لضمان التوافق مع عملاء البريد الإلكتروني
4. قم بتوثيق العناصر النائبة المستخدمة في القالب

### نصائح لتصميم قوالب البريد الإلكتروني

- استخدم CSS المضمن بدلاً من الأوراق الخارجية
- تجنب استخدام JavaScript
- استخدم تصميم بسيط وجداول لضمان التوافق مع مختلف عملاء البريد الإلكتروني
- اختبر القوالب في مختلف عملاء البريد الإلكتروني

## الاختبار

يمكن اختبار قوالب البريد الإلكتروني باستخدام `MockEmailSender` الذي يقوم بتخزين رسائل البريد الإلكتروني المرسلة بدلاً من إرسالها فعلياً. هذا مفيد في بيئات الاختبار والتطوير.

```csharp
// في اختبارات الوحدة
var mockEmailSender = new MockEmailSender();
var emailTemplateService = new EmailTemplateService("path/to/templates");
var service = new YourService(mockEmailSender, emailTemplateService);

// تنفيذ العملية التي ترسل بريداً إلكترونياً
await service.DoSomethingThatSendsEmail();

// التحقق من إرسال البريد الإلكتروني
Assert.Equal(1, mockEmailSender.SentEmails.Count);
var email = mockEmailSender.SentEmails.First();
Assert.Contains("ExpectedText", email.Body);
```

## الخلاصة

nنظام قوالب البريد الإلكتروني يوفر طريقة مرنة وقابلة للصيانة لإدارة محتوى البريد الإلكتروني في تطبيق DigitekShop. من خلال فصل محتوى البريد الإلكتروني عن الكود، يمكن تحديث وتخصيص رسائل البريد الإلكتروني بسهولة دون الحاجة إلى تغيير الكود أو إعادة نشر التطبيق.