using DigitekShop.Domain.Entities.Common;

namespace DigitekShop.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Read Operations
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T> GetByIdWithIncludesAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<(IEnumerable<T> Items, int TotalCount)> GetActivePagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        
        // Create Operations
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        
        // Update Operations
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        
        // Delete Operations
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task RestoreAsync(int id, CancellationToken cancellationToken = default);
        Task RestoreAsync(T entity, CancellationToken cancellationToken = default);
        
        // Existence Checks
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default);
        
        // Count Operations
        Task<int> GetTotalCountAsync();
        Task<int> GetActiveCountAsync();
    }
} 