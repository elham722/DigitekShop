namespace DigitekShop.Domain.Interfaces
{
    /// <summary>
    /// Unit of Work pattern interface for managing transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Repository Properties
        IProductRepository Products { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        ICategoryRepository Categories { get; }
        IBrandRepository Brands { get; }
        IReviewRepository Reviews { get; }
        IWishlistRepository Wishlists { get; }

        // Transaction Management
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        
        // Generic Repository
        IGenericRepository<T> Repository<T>() where T : Entities.Common.BaseEntity;
    }
} 