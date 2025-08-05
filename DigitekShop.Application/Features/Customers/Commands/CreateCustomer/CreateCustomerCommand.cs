using System;
using DigitekShop.Application.DTOs.Customer;
using DigitekShop.Application.Interfaces;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.Features.Customers.Commands.CreateCustomer
{
    public record CreateCustomerCommand : ICommand<CustomerDto>
    {
        // Required Information
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;

        // Optional Personal Information
        public string? MiddleName { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? Gender { get; init; }
        public string? NationalCode { get; init; }
        public string? PassportNumber { get; init; }

        // Address Information
        public string? Address { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? Country { get; init; }
        public string? PostalCode { get; init; }

        // Profile Information
        public string? Bio { get; init; }
        public string? Website { get; init; }
        public string? Company { get; init; }
        public string? JobTitle { get; init; }

        // Business Information
        public CustomerType Type { get; init; } = CustomerType.Regular;

        // Integration with Identity
        public string? UserId { get; init; }

        // Notes
        public string? Notes { get; init; }
    }
} 