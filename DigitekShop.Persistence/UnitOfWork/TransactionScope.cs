using DigitekShop.Domain.Interfaces;

namespace DigitekShop.Persistence.UnitOfWork
{
    /// <summary>
    /// Helper class for managing transactions with automatic disposal
    /// </summary>
    public class TransactionScope : IAsyncDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private bool _disposed;

        public TransactionScope(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Begins a transaction scope
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork instance</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>TransactionScope instance</returns>
        public static async Task<TransactionScope> BeginAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            return new TransactionScope(unitOfWork);
        }

        /// <summary>
        /// Commits the transaction
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (!_disposed)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (!_disposed)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (_unitOfWork.HasActiveTransaction)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                }
                await _unitOfWork.DisposeAsync();
                _disposed = true;
            }
        }
    }
} 