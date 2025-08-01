# DigitekShop Infrastructure Layer

این لایه شامل پیاده‌سازی سرویس‌های Infrastructure و third-party integrations است.

## Email Service

### تنظیمات

برای استفاده از سرویس ایمیل، تنظیمات زیر را در `appsettings.json` اضافه کنید:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "EnableSsl": true,
    "FromEmail": "your-email@gmail.com",
    "FromName": "DigitekShop System"
  }
}
```

### ثبت سرویس‌ها

در `Program.cs` یا `Startup.cs`:

```csharp
using DigitekShop.Infrastructure;

// Register Infrastructure services
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
```

### استفاده از سرویس ایمیل

```csharp
public class SomeService
{
    private readonly IEmailSender _emailSender;

    public SomeService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task SendWelcomeEmail(string userEmail, string userName)
    {
        var subject = "خوش آمدید!";
        var body = $"سلام {userName}، به DigitekShop خوش آمدید!";
        
        var result = await _emailSender.SendEmailAsync(userEmail, subject, body, isHtml: false);
        
        if (result)
        {
            // Email sent successfully
        }
    }
}
```

## تنظیمات Gmail

برای استفاده از Gmail:

1. در Gmail خود 2-Step Verification را فعال کنید
2. یک App Password ایجاد کنید 
3. از App Password به جای پسورد اصلی استفاده کنید

## تنظیمات سایر ارائه‌دهندگان

### Outlook/Hotmail
- SmtpServer: "smtp-mail.outlook.com"
- SmtpPort: 587
- EnableSsl: true

### Yahoo
- SmtpServer: "smtp.mail.yahoo.com"
- SmtpPort: 587
- EnableSsl: true

### SendGrid
- SmtpServer: "smtp.sendgrid.net"
- SmtpPort: 587
- SmtpUsername: "apikey"
- SmtpPassword: "your-sendgrid-api-key"