using DigitekShop.Application.DTOs.Customer;
using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Features.Customers.Commands.CreateCustomer
{
    public record CreateCustomerCommand : ICommand<CustomerDto>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public DateTime? DateOfBirth { get; init; }
        public string NationalCode { get; init; } = string.Empty;
        public string? ProfileImageUrl { get; init; }
        public string? Notes { get; init; }
        
        // Address information
        public string? Street { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? PostalCode { get; init; }
        public string? Country { get; init; }
    }
} 