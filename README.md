# DigitekShop - Online Electronics Store

A modern online electronics store built with .NET Core, following Clean Architecture and Domain-Driven Design (DDD) principles.

## 🏗️ Architecture

This project follows Clean Architecture principles with a strong focus on Domain-Driven Design:

```
DigitekShop/
├── DigitekShop.Domain/          # Domain Layer (Core Business Logic)
├── DigitekShop.Application/     # Application Layer (Use Cases)
├── DigitekShop.Infrastructure/  # Infrastructure Layer (External Concerns)
└── DigitekShop.Web/            # Presentation Layer (API/Web)
```

## 🎯 Domain Layer Features

### Entities
- **Product**: Electronics products with specifications
- **Order**: Customer orders with items
- **Customer**: User accounts and profiles
- **Category**: Product categorization
- **Brand**: Product brands
- **Review**: Product reviews and ratings
- **Wishlist**: Customer wishlists
- **OrderItem**: Individual items in orders

### Value Objects
- **Money**: Currency and amount handling
- **Email**: Email validation and formatting
- **PhoneNumber**: Iranian phone number support
- **Address**: Iranian address structure
- **ProductName**: Product naming conventions
- **SKU**: Stock keeping unit
- **OrderNumber**: Unique order identifiers

### Domain Services
- **OrderDomainService**: Order-related business logic
- **ProductDomainService**: Product-related business logic
- **DiscountCalculatorService**: Discount calculations

### Specifications
- **ProductSpecifications**: Product filtering and querying
- **OrderSpecifications**: Order filtering and querying

### Business Rules
- **OrderBusinessRules**: Order validation rules
- **BusinessRuleValidator**: Rule validation engine

### Policies
- **DiscountPolicies**: Various discount strategies
- **IDiscountPolicy**: Discount policy interface

### Domain Events
- **OrderCreatedEvent**: Order creation notifications
- **ProductCreatedEvent**: Product creation notifications
- **OrderStatusChangedEvent**: Status change notifications
- **ProductStockUpdatedEvent**: Stock update notifications
- **CustomerRegisteredEvent**: Registration notifications

### Aggregates
- **OrderAggregate**: Order aggregate root

### Exceptions
- **DomainException**: Base domain exception
- **ProductNotFoundException**: Product not found
- **OrderNotFoundException**: Order not found
- **CustomerNotFoundException**: Customer not found
- **InsufficientStockException**: Stock validation
- **InvalidOrderStatusException**: Status transition validation

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/DigitekShop.git
cd DigitekShop
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

## 📁 Project Structure

### Domain Layer (`DigitekShop.Domain`)
The core business logic layer containing:
- **Entities**: Core business objects
- **Value Objects**: Immutable objects representing concepts
- **Domain Services**: Business logic that spans multiple entities
- **Specifications**: Query specifications for filtering
- **Business Rules**: Validation rules
- **Policies**: Business policies and strategies
- **Domain Events**: Event-driven architecture support
- **Aggregates**: Aggregate roots for consistency
- **Exceptions**: Domain-specific exceptions

### Application Layer (`DigitekShop.Application`)
The application layer containing:
- **DTOs**: Data transfer objects
- **Profiles**: AutoMapper profiles
- **Use Cases**: Application services and handlers

## 🎨 Design Patterns

### Domain-Driven Design (DDD)
- **Entities**: Rich domain models with behavior
- **Value Objects**: Immutable objects for concepts
- **Aggregates**: Consistency boundaries
- **Domain Services**: Business logic services
- **Specifications**: Query specifications
- **Domain Events**: Event-driven architecture

### Clean Architecture
- **Dependency Inversion**: High-level modules don't depend on low-level modules
- **Separation of Concerns**: Clear boundaries between layers
- **Testability**: Easy to test business logic in isolation

## 🔧 Development

### Adding New Features
1. Start with the Domain layer
2. Define entities and value objects
3. Implement business rules and validation
4. Add domain services if needed
5. Create specifications for querying
6. Implement in Application layer
7. Add infrastructure concerns

### Testing
The domain layer is designed for easy testing:
- Value objects are immutable and easily testable
- Business rules can be tested in isolation
- Domain services have clear contracts
- Specifications can be tested independently

## 📝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🤝 Support

If you have any questions or need help, please open an issue on GitHub.

---

**Note**: This project is currently in development. The Domain layer is complete with comprehensive DDD patterns, and the Application layer is being developed. 