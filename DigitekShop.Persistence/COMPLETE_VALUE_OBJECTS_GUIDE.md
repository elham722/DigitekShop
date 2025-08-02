# Complete Value Objects Configuration Guide

## Overview
This document provides a comprehensive guide for all Value Objects configured in the DigitekShop project using Entity Framework Core's Owned Entity Types.

## All Value Objects Configuration

### 1. Customer Entity Value Objects

#### Address Value Object
```csharp
entity.OwnsOne(c => c.Address, address =>
{
    address.Property(a => a.Province).HasColumnName("Province").IsRequired();
    address.Property(a => a.City).HasColumnName("City").IsRequired();
    address.Property(a => a.District).HasColumnName("District").IsRequired();
    address.Property(a => a.Street).HasColumnName("Street").IsRequired();
    address.Property(a => a.Alley).HasColumnName("Alley");
    address.Property(a => a.Building).HasColumnName("Building");
    address.Property(a => a.Unit).HasColumnName("Unit");
    address.Property(a => a.PostalCode).HasColumnName("PostalCode");
    address.Property(a => a.Description).HasColumnName("Description");
});
```

#### Email Value Object
```csharp
entity.OwnsOne(c => c.Email, email =>
{
    email.Property(e => e.Value).HasColumnName("Email").IsRequired();
});
```

#### Phone Value Object
```csharp
entity.OwnsOne(c => c.Phone, phone =>
{
    phone.Property(p => p.Value).HasColumnName("Phone").IsRequired();
});
```

### 2. Product Entity Value Objects

#### ProductName Value Object
```csharp
entity.OwnsOne(p => p.Name, name =>
{
    name.Property(n => n.Value).HasColumnName("Name").IsRequired();
});
```

#### SKU Value Object
```csharp
entity.OwnsOne(p => p.SKU, sku =>
{
    sku.Property(s => s.Value).HasColumnName("SKU").IsRequired();
});
```

#### Price Value Object (Money)
```csharp
entity.OwnsOne(p => p.Price, money =>
{
    money.Property(m => m.Amount).HasColumnName("Price").IsRequired();
    money.Property(m => m.Currency).HasColumnName("Currency").IsRequired();
});
```

### 3. Brand Entity Value Objects

#### ProductName Value Object
```csharp
entity.OwnsOne(b => b.Name, name =>
{
    name.Property(n => n.Value).HasColumnName("Name").IsRequired();
});
```

### 4. Category Entity Value Objects

#### ProductName Value Object
```csharp
entity.OwnsOne(c => c.Name, name =>
{
    name.Property(n => n.Value).HasColumnName("Name").IsRequired();
});
```

### 5. Order Entity Value Objects

#### OrderNumber Value Object
```csharp
entity.OwnsOne(o => o.OrderNumber, orderNumber =>
{
    orderNumber.Property(on => on.Value).HasColumnName("OrderNumber").IsRequired();
});
```

#### ShippingAddress Value Object
```csharp
entity.OwnsOne(o => o.ShippingAddress, address =>
{
    address.Property(a => a.Province).HasColumnName("ShippingProvince").IsRequired();
    address.Property(a => a.City).HasColumnName("ShippingCity").IsRequired();
    address.Property(a => a.District).HasColumnName("ShippingDistrict").IsRequired();
    address.Property(a => a.Street).HasColumnName("ShippingStreet").IsRequired();
    address.Property(a => a.Alley).HasColumnName("ShippingAlley");
    address.Property(a => a.Building).HasColumnName("ShippingBuilding");
    address.Property(a => a.Unit).HasColumnName("ShippingUnit");
    address.Property(a => a.PostalCode).HasColumnName("ShippingPostalCode");
    address.Property(a => a.Description).HasColumnName("ShippingDescription");
});
```

#### BillingAddress Value Object
```csharp
entity.OwnsOne(o => o.BillingAddress, address =>
{
    address.Property(a => a.Province).HasColumnName("BillingProvince").IsRequired();
    address.Property(a => a.City).HasColumnName("BillingCity").IsRequired();
    address.Property(a => a.District).HasColumnName("BillingDistrict").IsRequired();
    address.Property(a => a.Street).HasColumnName("BillingStreet").IsRequired();
    address.Property(a => a.Alley).HasColumnName("BillingAlley");
    address.Property(a => a.Building).HasColumnName("BillingBuilding");
    address.Property(a => a.Unit).HasColumnName("BillingUnit");
    address.Property(a => a.PostalCode).HasColumnName("BillingPostalCode");
    address.Property(a => a.Description).HasColumnName("BillingDescription");
});
```

#### TotalAmount Value Object (Money)
```csharp
entity.OwnsOne(o => o.TotalAmount, money =>
{
    money.Property(m => m.Amount).HasColumnName("TotalAmount").IsRequired();
    money.Property(m => m.Currency).HasColumnName("TotalCurrency").IsRequired();
});
```

#### ShippingCost Value Object (Money)
```csharp
entity.OwnsOne(o => o.ShippingCost, money =>
{
    money.Property(m => m.Amount).HasColumnName("ShippingCost").IsRequired();
    money.Property(m => m.Currency).HasColumnName("ShippingCurrency").IsRequired();
});
```

#### TaxAmount Value Object (Money)
```csharp
entity.OwnsOne(o => o.TaxAmount, money =>
{
    money.Property(m => m.Amount).HasColumnName("TaxAmount").IsRequired();
    money.Property(m => m.Currency).HasColumnName("TaxCurrency").IsRequired();
});
```

#### DiscountAmount Value Object (Money)
```csharp
entity.OwnsOne(o => o.DiscountAmount, money =>
{
    money.Property(m => m.Amount).HasColumnName("DiscountAmount").IsRequired();
    money.Property(m => m.Currency).HasColumnName("DiscountCurrency").IsRequired();
});
```

#### FinalAmount Value Object (Money)
```csharp
entity.OwnsOne(o => o.FinalAmount, money =>
{
    money.Property(m => m.Amount).HasColumnName("FinalAmount").IsRequired();
    money.Property(m => m.Currency).HasColumnName("FinalCurrency").IsRequired();
});
```

### 6. OrderItem Entity Value Objects

#### UnitPrice Value Object (Money)
```csharp
entity.OwnsOne(oi => oi.UnitPrice, money =>
{
    money.Property(m => m.Amount).HasColumnName("UnitPrice").IsRequired();
    money.Property(m => m.Currency).HasColumnName("UnitCurrency").IsRequired();
});
```

#### TotalPrice Value Object (Money)
```csharp
entity.OwnsOne(oi => oi.TotalPrice, money =>
{
    money.Property(m => m.Amount).HasColumnName("TotalPrice").IsRequired();
    money.Property(m => m.Currency).HasColumnName("TotalCurrency").IsRequired();
});
```

#### DiscountAmount Value Object (Money)
```csharp
entity.OwnsOne(oi => oi.DiscountAmount, money =>
{
    money.Property(m => m.Amount).HasColumnName("DiscountAmount").IsRequired();
    money.Property(m => m.Currency).HasColumnName("DiscountCurrency").IsRequired();
});
```

## Database Schema Overview

### Customer Table
```sql
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NOT NULL,           -- Email Value Object
    Phone NVARCHAR(20) NOT NULL,            -- Phone Value Object
    Province NVARCHAR(50) NOT NULL,         -- Address Value Object
    City NVARCHAR(50) NOT NULL,
    District NVARCHAR(50) NOT NULL,
    Street NVARCHAR(100) NOT NULL,
    Alley NVARCHAR(50),
    Building NVARCHAR(50),
    Unit NVARCHAR(20),
    PostalCode NVARCHAR(10),
    Description NVARCHAR(500),
    Status INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);
```

### Product Table
```sql
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,            -- ProductName Value Object
    SKU NVARCHAR(50) NOT NULL,              -- SKU Value Object
    Description NVARCHAR(1000),
    Price DECIMAL(18,2) NOT NULL,           -- Money Value Object
    Currency NVARCHAR(3) NOT NULL,
    StockQuantity INT NOT NULL,
    CategoryId INT,
    BrandId INT,
    Status INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);
```

### Brand Table
```sql
CREATE TABLE Brands (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,            -- ProductName Value Object
    Description NVARCHAR(1000),
    LogoUrl NVARCHAR(500),
    Website NVARCHAR(500),
    Country NVARCHAR(100),
    FoundedYear INT,
    IsActive BIT NOT NULL,
    DisplayOrder INT NOT NULL,
    Slug NVARCHAR(200),
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);
```

### Category Table
```sql
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,            -- ProductName Value Object
    Description NVARCHAR(1000),
    ImageUrl NVARCHAR(500),
    Type INT NOT NULL,
    DisplayOrder INT NOT NULL,
    Slug NVARCHAR(200),
    ParentCategoryId INT,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);
```

### Order Table
```sql
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderNumber NVARCHAR(50) NOT NULL,      -- OrderNumber Value Object
    CustomerId INT NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,     -- Money Value Object
    TotalCurrency NVARCHAR(3) NOT NULL,
    ShippingCost DECIMAL(18,2) NOT NULL,    -- Money Value Object
    ShippingCurrency NVARCHAR(3) NOT NULL,
    TaxAmount DECIMAL(18,2) NOT NULL,       -- Money Value Object
    TaxCurrency NVARCHAR(3) NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL,  -- Money Value Object
    DiscountCurrency NVARCHAR(3) NOT NULL,
    FinalAmount DECIMAL(18,2) NOT NULL,     -- Money Value Object
    FinalCurrency NVARCHAR(3) NOT NULL,
    ShippingProvince NVARCHAR(50) NOT NULL, -- Address Value Object
    ShippingCity NVARCHAR(50) NOT NULL,
    ShippingDistrict NVARCHAR(50) NOT NULL,
    ShippingStreet NVARCHAR(100) NOT NULL,
    ShippingAlley NVARCHAR(50),
    ShippingBuilding NVARCHAR(50),
    ShippingUnit NVARCHAR(20),
    ShippingPostalCode NVARCHAR(10),
    ShippingDescription NVARCHAR(500),
    BillingProvince NVARCHAR(50) NOT NULL,  -- Address Value Object
    BillingCity NVARCHAR(50) NOT NULL,
    BillingDistrict NVARCHAR(50) NOT NULL,
    BillingStreet NVARCHAR(100) NOT NULL,
    BillingAlley NVARCHAR(50),
    BillingBuilding NVARCHAR(50),
    BillingUnit NVARCHAR(20),
    BillingPostalCode NVARCHAR(10),
    BillingDescription NVARCHAR(500),
    Status INT NOT NULL,
    PaymentMethod INT NOT NULL,
    ShippingMethod NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);
```

### OrderItem Table
```sql
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,       -- Money Value Object
    UnitCurrency NVARCHAR(3) NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,      -- Money Value Object
    TotalCurrency NVARCHAR(3) NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL,  -- Money Value Object
    DiscountCurrency NVARCHAR(3) NOT NULL,
    DiscountPercentage DECIMAL(5,2) NOT NULL,
    Notes NVARCHAR(500),
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);
```

## Value Objects Summary

| Entity | Value Object | Type | Usage |
|--------|-------------|------|-------|
| Customer | Address | Complex | Customer address |
| Customer | Email | Simple | Customer email |
| Customer | Phone | Simple | Customer phone |
| Product | ProductName | Simple | Product name |
| Product | SKU | Simple | Product SKU |
| Product | Price | Money | Product price |
| Brand | ProductName | Simple | Brand name |
| Category | ProductName | Simple | Category name |
| Order | OrderNumber | Simple | Order number |
| Order | ShippingAddress | Complex | Shipping address |
| Order | BillingAddress | Complex | Billing address |
| Order | TotalAmount | Money | Order total |
| Order | ShippingCost | Money | Shipping cost |
| Order | TaxAmount | Money | Tax amount |
| Order | DiscountAmount | Money | Discount amount |
| Order | FinalAmount | Money | Final amount |
| OrderItem | UnitPrice | Money | Unit price |
| OrderItem | TotalPrice | Money | Total price |
| OrderItem | DiscountAmount | Money | Discount amount |

## Migration Commands

### Create Initial Migration
```bash
cd DigitekShop.Persistence
dotnet ef migrations add InitialCreate --startup-project ../DigitekShop.Api
```

### Update Database
```bash
dotnet ef database update --startup-project ../DigitekShop.Api
```

### Generate SQL Script
```bash
dotnet ef migrations script --startup-project ../DigitekShop.Api --output migration.sql
```

## Troubleshooting

### Common Issues

1. **"Unable to determine the relationship"**
   - Ensure all Value Objects are configured in `ConfigureValueObjects`
   - Check property names match exactly

2. **"Column not found"**
   - Verify column names in configuration
   - Check database schema

3. **"Primary key required"**
   - Ensure Value Objects are configured as owned entity types
   - Don't register Value Objects as separate entities

### Best Practices

1. **Consistent Naming**: Use consistent column naming conventions
2. **Currency Handling**: Always include currency for Money Value Objects
3. **Validation**: Keep validation logic in Value Object constructors
4. **Testing**: Test Value Object creation and validation
5. **Documentation**: Keep this guide updated with any changes

## Future Enhancements

1. **Audit Trail**: Add audit fields to Value Objects
2. **Caching**: Implement caching for frequently used Value Objects
3. **Validation**: Add more sophisticated validation rules
4. **Localization**: Support multiple languages in Value Objects
5. **Performance**: Optimize Value Object operations 