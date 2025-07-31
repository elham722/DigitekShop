using DigitekShop.Domain.Entities.Common;

namespace DigitekShop.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Read Operations
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetActiveAsync();
        
        // Create Operations
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        
        // Update Operations
        Task<T> UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        
        // Delete Operations
        Task DeleteAsync(int id);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task SoftDeleteAsync(int id);
        Task SoftDeleteAsync(T entity);
        Task RestoreAsync(int id);
        Task RestoreAsync(T entity);
        
        // Existence Checks
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(T entity);
        Task<bool> ExistsActiveAsync(int id);
        
        // Count Operations
        Task<int> GetTotalCountAsync();
        Task<int> GetActiveCountAsync();
    }
} 