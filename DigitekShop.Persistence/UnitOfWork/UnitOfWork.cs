using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Persistence.Contexts;
using DigitekShop.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DigitekShop.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DigitekShopDBContext _context;
        private IDbContextTransaction? _currentTransaction;

        // Repositories
        public IProductRepository Products { get; }
        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }
        public ICategoryRepository Categories { get; }
        public IBrandRepository Brands { get; }
        public IReviewRepository Reviews { get; }
        public IWishlistRepository Wishlists { get; }

        public UnitOfWork(DigitekShopDBContext context)
        {
            _context = context;
            
            // Initialize repositories
            Products = new ProductRepository(context);
            Customers = new CustomerRepository(context);
            Orders = new OrderRepository(context);
            Categories = new CategoryRepository(context);
            Brands = new BrandRepository(context);
            Reviews = new ReviewRepository(context);
            Wishlists = new WishlistRepository(context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            }
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch
            {
                // Log the rollback error but don't throw
                // This prevents masking the original exception
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
            _currentTransaction?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
            
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }
        }

        private readonly Dictionary<Type, object> _repositories = new();

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T);

            if (_repositories.ContainsKey(type))
            {
                return (IGenericRepository<T>)_repositories[type];
            }

            // اگر ریپوزیتوری اختصاصی وجود دارد، از آن استفاده کن، وگرنه Generic بساز
            object repositoryInstance;

            if (type == typeof(Product))
                repositoryInstance = Products;
            else if (type == typeof(Customer))
                repositoryInstance = Customers;
            else if (type == typeof(Order))
                repositoryInstance = Orders;
            else if (type == typeof(Category))
                repositoryInstance = Categories;
            else if (type == typeof(Brand))
                repositoryInstance = Brands;
            else if (type == typeof(Review))
                repositoryInstance = Reviews;
            else if (type == typeof(Wishlist))
                repositoryInstance = Wishlists;
            else
                repositoryInstance = new GenericRepository<T>(_context);

            _repositories[type] = repositoryInstance;

            return (IGenericRepository<T>)repositoryInstance;
        }

    }
} 