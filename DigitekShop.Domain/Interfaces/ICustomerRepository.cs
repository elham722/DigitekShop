using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(Email email);
        Task<Customer> GetByPhoneAsync(PhoneNumber phone);
        Task<IEnumerable<Customer>> GetByStatusAsync(CustomerStatus status);
        Task<IEnumerable<Customer>> GetActiveCustomersAsync();
        Task<IEnumerable<Customer>> SearchByNameAsync(string searchTerm);
        Task<bool> ExistsByEmailAsync(Email email);
        Task<bool> ExistsByPhoneAsync(PhoneNumber phone);
    }
} 