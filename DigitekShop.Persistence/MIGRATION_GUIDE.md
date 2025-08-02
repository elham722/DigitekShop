# Entity Framework Migration Guide

## Overview
This guide explains how to create and manage database migrations for the DigitekShop project.

## Prerequisites

1. **SQL Server**: Make sure SQL Server is installed and running
2. **Connection String**: Verify your connection string in `appsettings.json`
3. **EF Core Tools**: Install Entity Framework Core tools globally

```bash
dotnet tool install --global dotnet-ef
```

## Project Structure

```
DigitekShop.Persistence/
├── Contexts/
│   ├── DigitekShopDBContext.cs
│   └── DesignTimeDbContextFactory.cs
├── appsettings.json
├── appsettings.Development.json
└── MIGRATION_GUIDE.md
```

## Configuration Files

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DigitekDBConnection": "Data Source=.;Initial catalog=DigitekShop;Integrated security=true;TrustServerCertificate=True;"
  }
}
```

### appsettings.Development.json
```json
{
  "ConnectionStrings": {
    "DigitekDBConnection": "Data Source=.;Initial catalog=DigitekShop;Integrated security=true;TrustServerCertificate=True;"
  }
}
```

## Creating Migrations

### 1. Navigate to Persistence Project
```bash
cd DigitekShop.Persistence
```

### 2. Create Initial Migration
```bash
dotnet ef migrations add InitialCreate --startup-project ../DigitekShop.Api
```

### 3. Create Migration for Specific Changes
```bash
dotnet ef migrations add AddProductTable --startup-project ../DigitekShop.Api
dotnet ef migrations add AddCustomerTable --startup-project ../DigitekShop.Api
dotnet ef migrations add AddOrderTable --startup-project ../DigitekShop.Api
```

## Applying Migrations

### 1. Update Database
```bash
dotnet ef database update --startup-project ../DigitekShop.Api
```

### 2. Update to Specific Migration
```bash
dotnet ef database update MigrationName --startup-project ../DigitekShop.Api
```

### 3. Update to Latest Migration
```bash
dotnet ef database update --startup-project ../DigitekShop.Api
```

## Managing Migrations

### 1. List Migrations
```bash
dotnet ef migrations list --startup-project ../DigitekShop.Api
```

### 2. Remove Last Migration
```bash
dotnet ef migrations remove --startup-project ../DigitekShop.Api
```

### 3. Generate SQL Script
```bash
dotnet ef migrations script --startup-project ../DigitekShop.Api
```

### 4. Generate SQL Script from Specific Migration
```bash
dotnet ef migrations script FromMigrationName ToMigrationName --startup-project ../DigitekShop.Api
```

## Troubleshooting

### Common Issues

#### 1. "Unable to create a 'DbContext'"
**Solution**: Make sure `DesignTimeDbContextFactory` is properly configured and connection string is correct.

#### 2. "Connection string not found"
**Solution**: Verify that `appsettings.json` exists in the Persistence project and contains the correct connection string.

#### 3. "Database does not exist"
**Solution**: Create the database manually or use:
```bash
dotnet ef database update --startup-project ../DigitekShop.Api
```

#### 4. "Migration already exists"
**Solution**: Remove the existing migration first:
```bash
dotnet ef migrations remove --startup-project ../DigitekShop.Api
```

### Connection String Examples

#### Local SQL Server
```json
{
  "ConnectionStrings": {
    "DigitekDBConnection": "Data Source=.;Initial catalog=DigitekShop;Integrated security=true;TrustServerCertificate=True;"
  }
}
```

#### SQL Server with Username/Password
```json
{
  "ConnectionStrings": {
    "DigitekDBConnection": "Data Source=.;Initial catalog=DigitekShop;User Id=YourUsername;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

#### SQL Server Express
```json
{
  "ConnectionStrings": {
    "DigitekDBConnection": "Data Source=.\\SQLEXPRESS;Initial catalog=DigitekShop;Integrated security=true;TrustServerCertificate=True;"
  }
}
```

#### Azure SQL Database
```json
{
  "ConnectionStrings": {
    "DigitekDBConnection": "Server=your-server.database.windows.net;Database=DigitekShop;User Id=YourUsername;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

## Best Practices

### 1. Migration Naming
- Use descriptive names: `AddProductTable`, `UpdateCustomerEmailField`
- Include the entity name in the migration name
- Use PascalCase

### 2. Migration Frequency
- Create migrations for each logical change
- Don't create too many small migrations
- Don't create one huge migration

### 3. Testing Migrations
- Always test migrations in development first
- Test both up and down migrations
- Verify data integrity after migration

### 4. Backup Strategy
- Always backup database before applying migrations in production
- Test migrations on production-like data
- Have a rollback plan

## Production Deployment

### 1. Generate SQL Script
```bash
dotnet ef migrations script --startup-project ../DigitekShop.Api --output migration.sql
```

### 2. Review Generated SQL
- Check the generated SQL script
- Verify all changes are correct
- Test on staging environment

### 3. Apply to Production
- Backup production database
- Run the SQL script
- Verify application functionality

## Development Workflow

### 1. Make Entity Changes
```csharp
// Modify your entity classes
public class Product
{
    public string NewProperty { get; set; }
}
```

### 2. Create Migration
```bash
dotnet ef migrations add AddNewPropertyToProduct --startup-project ../DigitekShop.Api
```

### 3. Review Migration
- Check the generated migration file
- Verify the changes are correct
- Test the migration

### 4. Apply Migration
```bash
dotnet ef database update --startup-project ../DigitekShop.Api
```

### 5. Test Application
- Run the application
- Verify new functionality works
- Check for any issues

## Advanced Topics

### 1. Seeding Data
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Seed data
    modelBuilder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Electronics" },
        new Category { Id = 2, Name = "Books" }
    );
}
```

### 2. Custom Migration Operations
```csharp
public partial class CustomMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Custom SQL operations
        migrationBuilder.Sql("UPDATE Products SET Status = 'Active' WHERE Status IS NULL");
    }
}
```

### 3. Migration History
```bash
# View migration history in database
dotnet ef migrations list --startup-project ../DigitekShop.Api
```

## Useful Commands Reference

| Command | Description |
|---------|-------------|
| `dotnet ef migrations add Name` | Create new migration |
| `dotnet ef migrations remove` | Remove last migration |
| `dotnet ef migrations list` | List all migrations |
| `dotnet ef database update` | Apply migrations to database |
| `dotnet ef database update MigrationName` | Update to specific migration |
| `dotnet ef migrations script` | Generate SQL script |
| `dotnet ef dbcontext info` | Show DbContext information |
| `dotnet ef dbcontext scaffold` | Reverse engineer from database |

## Environment-Specific Configuration

### Development
- Use local SQL Server
- Enable detailed logging
- Use development connection string

### Staging
- Use staging database
- Test all migrations
- Verify data integrity

### Production
- Use production database
- Backup before migration
- Monitor migration process
- Have rollback plan ready 