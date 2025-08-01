# إصلاح مشكلة الاعتماد المعكوس في خدمة قوالب البريد الإلكتروني

## المشكلة

كانت طبقة التطبيق (Application Layer) تعتمد بشكل مباشر على تنفيذ محدد من طبقة البنية التحتية (Infrastructure Layer)، وهو `EmailTemplateService`. هذا يخالف مبدأ انعكاس الاعتماد (Dependency Inversion Principle) من مبادئ SOLID، حيث يجب أن تعتمد الطبقات عالية المستوى (مثل طبقة التطبيق) على التجريدات (Abstractions) وليس على التنفيذات المحددة (Concrete Implementations).

## الحل

تم تطبيق الحل التالي لإصلاح مشكلة الاعتماد المعكوس:

1. **إنشاء واجهة في طبقة التطبيق**: تم إنشاء واجهة `IEmailTemplateService` في طبقة التطبيق تحدد العقد الذي يجب أن يلتزم به أي تنفيذ لخدمة قوالب البريد الإلكتروني.

2. **تعديل التنفيذ في طبقة البنية التحتية**: تم تعديل `EmailTemplateService` في طبقة البنية التحتية لتنفيذ الواجهة `IEmailTemplateService`.

3. **تعديل الاعتمادات في طبقة التطبيق**: تم تعديل الأصناف في طبقة التطبيق لاستخدام الواجهة `IEmailTemplateService` بدلاً من التنفيذ المحدد `EmailTemplateService`.

4. **تعديل تسجيل الخدمات**: تم تعديل تسجيل الخدمات في `InfrastructureServiceRegistration` لتسجيل `EmailTemplateService` كتنفيذ للواجهة `IEmailTemplateService`.

## التغييرات التفصيلية

### 1. إنشاء واجهة IEmailTemplateService

```csharp
// DigitekShop.Application/Interfaces/Infrastructure/IEmailTemplateService.cs
namespace DigitekShop.Application.Interfaces.Infrastructure
{
    public interface IEmailTemplateService
    {
        Task<string> GetProcessedTemplateAsync(string templateName, Dictionary<string, string> replacements);
        Task<string> GetTemplateContentAsync(string templateName);
        void ClearCache();
    }
}
```

### 2. تعديل EmailTemplateService لتنفيذ الواجهة

```csharp
// DigitekShop.Infrastructure/Email/EmailTemplateService.cs
using DigitekShop.Application.Interfaces.Infrastructure;

namespace DigitekShop.Infrastructure.Email
{
    public class EmailTemplateService : IEmailTemplateService
    {
        // التنفيذ الحالي...
    }
}
```

### 3. تعديل الاعتمادات في طبقة التطبيق

```csharp
// DigitekShop.Application/Features/Customers/Commands/RegisterCustomer/RegisterCustomerCommandHandler.cs
using DigitekShop.Application.Interfaces.Infrastructure;

namespace DigitekShop.Application.Features.Customers.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandHandler
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateService _emailTemplateService;

        public RegisterCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailSender emailSender,
            IEmailTemplateService emailTemplateService)
        {
            // ...
        }

        // ...
    }
}
```

```csharp
// DigitekShop.Application/Features/Orders/Commands/CreateOrder/OrderConfirmationEmailService.cs
using DigitekShop.Application.Interfaces.Infrastructure;

namespace DigitekShop.Application.Features.Orders.Commands.CreateOrder
{
    public class OrderConfirmationEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateService _emailTemplateService;

        public OrderConfirmationEmailService(IEmailSender emailSender, IEmailTemplateService emailTemplateService)
        {
            // ...
        }

        // ...
    }
}
```

### 4. تعديل تسجيل الخدمات

```csharp
// DigitekShop.Infrastructure/InfrastructureServiceRegistration.cs
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ...
        
        // Register Email Template Service
        services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
        
        // ...
    }
}
```

## الفوائد

1. **الالتزام بمبدأ انعكاس الاعتماد**: الطبقات عالية المستوى تعتمد الآن على التجريدات وليس على التنفيذات المحددة.

2. **تحسين قابلية الاختبار**: يمكن الآن اختبار طبقة التطبيق بشكل معزول باستخدام تنفيذات وهمية للواجهة `IEmailTemplateService`.

3. **تحسين المرونة**: يمكن الآن تغيير تنفيذ خدمة قوالب البريد الإلكتروني دون التأثير على طبقة التطبيق.

4. **تحسين قابلية التوسع**: يمكن الآن إضافة تنفيذات جديدة للواجهة `IEmailTemplateService` دون تغيير الكود الموجود.

## الاختبارات

تم إنشاء اختبارات للتأكد من صحة التغييرات:

1. **اختبارات وحدة لطبقة التطبيق**: تستخدم تنفيذات وهمية للواجهة `IEmailTemplateService`.

2. **تنفيذ وهمي مخصص**: تم إنشاء تنفيذ وهمي مخصص `MockEmailTemplateService` للاستخدام في الاختبارات.

## الخلاصة

تم إصلاح مشكلة الاعتماد المعكوس في خدمة قوالب البريد الإلكتروني من خلال تطبيق مبدأ انعكاس الاعتماد. الآن، تعتمد طبقة التطبيق على واجهة `IEmailTemplateService` المعرفة في طبقة التطبيق نفسها، بدلاً من الاعتماد المباشر على تنفيذ محدد من طبقة البنية التحتية.