using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DigitekShopDBContext _context;

        public CustomerRepository(DigitekShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .AnyAsync(c => c.Email.Value == email.Value, cancellationToken);
        }

        public async Task<bool> ExistsByPhoneAsync(PhoneNumber phone, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .AnyAsync(c => c.Phone.Value == phone.Value, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Where(c => c.Status == CustomerStatus.Active)
                .ToListAsync(cancellationToken);
        }

        public async Task<Customer> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .Include(c => c.Reviews)
                .Include(c => c.Wishlist)
                .FirstOrDefaultAsync(c => c.Email.Value == email.Value, cancellationToken);
        }

        public async Task<Customer> GetByPhoneAsync(PhoneNumber phone, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .Include(c => c.Reviews)
                .Include(c => c.Wishlist)
                .FirstOrDefaultAsync(c => c.Phone.Value == phone.Value, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetByStatusAsync(CustomerStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Where(c => c.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Customer>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Customer>();

            searchTerm = searchTerm.Trim().ToLower();

            return await _context.Customers
                .Where(c =>
                    c.FirstName.ToLower().Contains(searchTerm) ||
                    c.LastName.ToLower().Contains(searchTerm))
                .ToListAsync(cancellationToken);
        }
    }

}