# Features Layer - CQRS Implementation

این لایه شامل پیاده‌سازی الگوی CQRS (Command Query Responsibility Segregation) برای مدیریت عملیات‌های مختلف سیستم است.

## ساختار پوشه‌ها

```
Features/
├── Products/
│   ├── Commands/
│   │   ├── CreateProduct/
│   │   ├── UpdateProduct/
│   │   └── DeleteProduct/
│   └── Queries/
│       ├── GetProduct/
│       └── GetProducts/
├── Customers/
│   ├── Commands/
│   │   └── CreateCustomer/
│   └── Queries/
├── Orders/
│   ├── Commands/
│   │   └── CreateOrder/
│   └── Queries/
└── FeaturesRegistration.cs
```

## الگوی CQRS

### Commands (دستورات)
- برای عملیات‌های تغییردهنده داده استفاده می‌شوند
- شامل: Create, Update, Delete
- هر Command یک Handler مخصوص دارد
- Business Logic در Handler پیاده‌سازی می‌شود

### Queries (پرس‌وجوها)
- برای عملیات‌های خواندن داده استفاده می‌شوند
- شامل: Get, GetAll, Search, Filter
- هر Query یک Handler مخصوص دارد
- فقط برای خواندن داده استفاده می‌شوند

## نحوه استفاده

### روش 1: استفاده مستقیم از Handler
```csharp
// ایجاد محصول جدید
var command = new CreateProductCommand
{
    Name = "لپ‌تاپ Dell",
    Description = "لپ‌تاپ گیمینگ",
    Price = 25000000,
    StockQuantity = 10,
    SKU = "DELL-001",
    CategoryId = 1
};

var handler = serviceProvider.GetService<ICommandHandler<CreateProductCommand, ProductDto>>();
var result = await handler.HandleAsync(command);
```

### روش 2: استفاده از Mediator (توصیه شده)
```csharp
// استفاده از Mediator
var mediator = serviceProvider.GetService<IMediator>();

// Command
var command = new CreateProductCommand { /* ... */ };
var result = await mediator.Send(command);

// Query
var query = new GetProductsQuery { /* ... */ };
var products = await mediator.Send(query);
```

## Interface Hierarchy

```
IRequest<TResult>
├── ICommand<TResult>
│   └── ICommand (ICommand<Unit>)
└── IQuery<TResult>

ICommandHandler<TCommand, TResult>
├── ICommandHandler<TCommand> (ICommandHandler<TCommand, Unit>)
└── IQueryHandler<TQuery, TResult>
```

## مزایای این معماری

1. **جداسازی مسئولیت‌ها**: عملیات‌های خواندن و نوشتن جدا هستند
2. **قابلیت بهینه‌سازی**: می‌توان Query و Command را جداگانه بهینه کرد
3. **قابلیت مقیاس‌پذیری**: می‌توان Query و Command را روی سرورهای مختلف اجرا کرد
4. **قابلیت تست**: هر Handler را می‌توان جداگانه تست کرد
5. **قابلیت نگهداری**: کد تمیز و قابل فهم است
6. **سازگاری با MediatR**: می‌توان در آینده به MediatR مهاجرت کرد

## ثبت در DI Container

تمام Handlers و Mediator در فایل `FeaturesRegistration.cs` ثبت می‌شوند و در `ApplicationServicesRegistration.cs` فراخوانی می‌شوند.

## مهاجرت به MediatR

برای استفاده از MediatR در آینده، کافی است:

1. `IMediator` را با `IMediator` از MediatR جایگزین کنید
2. `IRequest<TResult>` را با `IRequest<TResult>` از MediatR جایگزین کنید
3. Handlers را از `ICommandHandler` به `IRequestHandler` تغییر دهید 