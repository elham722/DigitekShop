using DigitekShop.Domain.Interfaces;

namespace DigitekShop.Persistence.UnitOfWork
{
    /// <summary>
    /// Factory interface for creating UnitOfWork instances
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Creates a new UnitOfWork instance
        /// </summary>
        /// <returns>New UnitOfWork instance</returns>
        IUnitOfWork Create();
        
        /// <summary>
        /// Creates a new UnitOfWork instance with transaction
        /// </summary>
        /// <returns>New UnitOfWork instance with active transaction</returns>
        Task<IUnitOfWork> CreateWithTransactionAsync(CancellationToken cancellationToken = default);
    }
} 