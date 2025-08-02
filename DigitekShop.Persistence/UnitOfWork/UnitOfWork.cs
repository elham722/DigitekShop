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

        public bool HasActiveTransaction => _currentTransaction != null;

        public string? GetCurrentTransactionId()
        {
            return _currentTransaction?.TransactionId.ToString();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public async Task<bool> HasChangesAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(_context.ChangeTracker.HasChanges());
        }

        public async Task ResetContextAsync(CancellationToken cancellationToken = default)
        {
            // Detach all entities
            DetachAllEntities();
            
            // Reset the context state
            _context.ChangeTracker.Clear();
            
            await Task.CompletedTask;
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                           e.State == EntityState.Modified ||
                           e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
            {
                entry.State = EntityState.Detached;
            }
        }

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<Task<TResult>> operation,
            CancellationToken cancellationToken = default)
        {
            if (HasActiveTransaction)
            {
                // اگر تراکنش فعال است، عملیات را مستقیماً اجرا کن
                return await operation();
            }

            await BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await operation();
                await SaveChangesAsync(cancellationToken);
                await CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task ExecuteInTransactionAsync(
            Func<Task> operation,
            CancellationToken cancellationToken = default)
        {
            if (HasActiveTransaction)
            {
                // اگر تراکنش فعال است، عملیات را مستقیماً اجرا کن
                await operation();
                return;
            }

            await BeginTransactionAsync(cancellationToken);
            try
            {
                await operation();
                await SaveChangesAsync(cancellationToken);
                await CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
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