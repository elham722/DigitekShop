# Value Objects Configuration in Entity Framework

## Overview
This document explains how Value Objects are configured in the DigitekShop project using Entity Framework Core's Owned Entity Types.

## Value Objects in the Project

### 1. Address
**Location**: `DigitekShop.Domain/ValueObjects/Address.cs`
**Usage**: Customer addresses, Order shipping/billing addresses

**Properties**:
- Province (required)
- City (required)
- District (required)
- Street (required)
- Alley (optional)
- Building (optional)
- Unit (optional)
- PostalCode (optional)
- Description (optional)

**Configuration**:
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

### 2. Money
**Location**: `DigitekShop.Domain/ValueObjects/Money.cs`
**Usage**: Product prices, Order amounts, Shipping costs

**Properties**:
- Amount (decimal)
- Currency (string, default: "IRR")

**Configuration**:
```csharp
entity.OwnsOne(p => p.Price, money =>
{
    money.Property(m => m.Amount).HasColumnName("Price").IsRequired();
    money.Property(m => m.Currency).HasColumnName("Currency").IsRequired();
});
```

### 3. Email
**Location**: `DigitekShop.Domain/ValueObjects/Email.cs`
**Usage**: Customer email addresses

**Properties**:
- Value (string, validated email format)

**Configuration**:
```csharp
entity.OwnsOne(c => c.Email, email =>
{
    email.Property(e => e.Value).HasColumnName("Email").IsRequired();
});
```

### 4. PhoneNumber
**Location**: `DigitekShop.Domain/ValueObjects/PhoneNumber.cs`
**Usage**: Customer phone numbers

**Properties**:
- Value (string, validated phone format)

**Configuration**:
```csharp
entity.OwnsOne(c => c.PhoneNumber, phone =>
{
    phone.Property(p => p.Value).HasColumnName("PhoneNumber").IsRequired();
});
```

### 5. ProductName
**Location**: `DigitekShop.Domain/ValueObjects/ProductName.cs`
**Usage**: Product names

**Properties**:
- Value (string, 3-100 characters)

**Configuration**:
```csharp
entity.OwnsOne(p => p.Name, name =>
{
    name.Property(n => n.Value).HasColumnName("Name").IsRequired();
});
```

### 6. SKU
**Location**: `DigitekShop.Domain/ValueObjects/SKU.cs`
**Usage**: Product SKU codes

**Properties**:
- Value (string, 5-50 characters, alphanumeric with hyphens)

**Configuration**:
```csharp
entity.OwnsOne(p => p.SKU, sku =>
{
    sku.Property(s => s.Value).HasColumnName("SKU").IsRequired();
});
```

### 7. OrderNumber
**Location**: `DigitekShop.Domain/ValueObjects/OrderNumber.cs`
**Usage**: Order identification numbers

**Properties**:
- Value (string, format: ORD-YYYYMMDDHHMMSS-XXXX)

**Configuration**:
```csharp
entity.OwnsOne(o => o.OrderNumber, orderNumber =>
{
    orderNumber.Property(on => on.Value).HasColumnName("OrderNumber").IsRequired();
});
```

## Database Schema

### Customer Table
```sql
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NOT NULL,           -- Email Value Object
    PhoneNumber NVARCHAR(20) NOT NULL,      -- PhoneNumber Value Object
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
    DiscountPrice DECIMAL(18,2),            -- Money Value Object
    DiscountCurrency NVARCHAR(3),
    StockQuantity INT NOT NULL,
    CategoryId INT,
    BrandId INT,
    Status INT NOT NULL,
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
    Currency NVARCHAR(3) NOT NULL,
    ShippingCost DECIMAL(18,2) NOT NULL,    -- Money Value Object
    ShippingCurrency NVARCHAR(3) NOT NULL,
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

## Benefits of Value Objects

### 1. Encapsulation
- Business logic is encapsulated within the Value Object
- Validation rules are enforced at the domain level
- Data integrity is maintained

### 2. Immutability
- Value Objects are immutable once created
- Prevents accidental modifications
- Ensures data consistency

### 3. Domain-Driven Design
- Represents domain concepts accurately
- Makes the code more expressive
- Improves maintainability

### 4. Type Safety
- Strong typing prevents errors
- Compile-time validation
- Better IntelliSense support

## Migration Considerations

### 1. Adding New Value Objects
When adding a new Value Object:

1. Create the Value Object class
2. Add configuration in `ConfigureValueObjects` method
3. Create migration
4. Test the migration

### 2. Modifying Existing Value Objects
When modifying existing Value Objects:

1. Update the Value Object class
2. Update the configuration if needed
3. Create migration for schema changes
4. Consider data migration if needed

### 3. Removing Value Objects
When removing Value Objects:

1. Remove the configuration
2. Create migration to drop columns
3. Remove the Value Object class
4. Update related code

## Best Practices

### 1. Validation
- Always validate input in Value Object constructors
- Throw meaningful exceptions for invalid data
- Use domain-specific validation rules

### 2. Immutability
- Make properties private set
- Provide business methods for operations
- Avoid setters that modify state

### 3. Equality
- Override `Equals` and `GetHashCode`
- Implement value-based equality
- Consider implementing `IEquatable<T>`

### 4. Serialization
- Ensure Value Objects can be serialized
- Consider JSON serialization attributes
- Test serialization in your API

### 5. Database Mapping
- Use descriptive column names
- Set appropriate constraints
- Consider indexing for frequently queried fields

## Testing Value Objects

### 1. Unit Tests
```csharp
[Test]
public void Address_WithValidData_ShouldCreateSuccessfully()
{
    // Arrange & Act
    var address = new Address("تهران", "تهران", "شمال", "ولیعصر");

    // Assert
    Assert.That(address.Province, Is.EqualTo("تهران"));
    Assert.That(address.City, Is.EqualTo("تهران"));
    Assert.That(address.District, Is.EqualTo("شمال"));
    Assert.That(address.Street, Is.EqualTo("ولیعصر"));
}
```

### 2. Validation Tests
```csharp
[Test]
public void Address_WithEmptyProvince_ShouldThrowException()
{
    // Arrange & Act & Assert
    Assert.Throws<ArgumentException>(() => 
        new Address("", "تهران", "شمال", "ولیعصر"));
}
```

### 3. Business Logic Tests
```csharp
[Test]
public void Address_IsInTehran_ShouldReturnTrue()
{
    // Arrange
    var address = new Address("تهران", "تهران", "شمال", "ولیعصر");

    // Act
    var result = address.IsInTehran();

    // Assert
    Assert.That(result, Is.True);
}
```

## Troubleshooting

### Common Issues

#### 1. "The entity type requires a primary key"
**Solution**: Ensure Value Objects are configured as owned entity types, not as separate entities.

#### 2. "Column not found"
**Solution**: Check that column names in configuration match the database schema.

#### 3. "Validation failed"
**Solution**: Ensure Value Object validation rules are appropriate for your data.

#### 4. "Migration conflicts"
**Solution**: Review migration files and ensure they're consistent with your configuration.

### Debugging Tips

1. **Check Configuration**: Verify all Value Objects are properly configured in `OnModelCreating`
2. **Review Migrations**: Check generated migration files for correct schema
3. **Test Validation**: Ensure Value Object validation works as expected
4. **Database Schema**: Verify the actual database schema matches your expectations

## Future Enhancements

1. **Audit Trail**: Add audit fields to Value Objects
2. **Caching**: Implement caching for frequently used Value Objects
3. **Validation**: Add more sophisticated validation rules
4. **Localization**: Support multiple languages in Value Objects
5. **Performance**: Optimize Value Object operations for better performance 