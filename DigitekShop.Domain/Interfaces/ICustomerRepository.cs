using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
        Task<Customer> GetByPhoneAsync(PhoneNumber phone, CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> GetByStatusAsync(CustomerStatus status, CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByPhoneAsync(PhoneNumber phone, CancellationToken cancellationToken = default);
    }
} 