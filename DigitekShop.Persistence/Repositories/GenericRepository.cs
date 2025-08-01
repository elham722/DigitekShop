using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DigitekShopDBContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DigitekShopDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;  // ذخیره نهایی تو UnitOfWork انجام میشه
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public virtual Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = _dbSet.Local.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                entity = _dbSet.Attach(entity).Entity;
            }
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public virtual async Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(e => e.Id == entity.Id, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(e => !e.IsDeleted).ToListAsync(cancellationToken);
        }

        public virtual async Task<int> GetActiveCountAsync()
        {
            return await _dbSet.CountAsync(e => !e.IsDeleted);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public virtual async Task<int> GetTotalCountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual Task RestoreAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = _dbSet.Local.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                entity = _dbSet.Attach(entity).Entity;
            }
            entity.IsDeleted = false;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual Task RestoreAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.IsDeleted = false;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

       public virtual Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = _dbSet.Local.FirstOrDefault(e => e.Id == id);
        if (entity == null)
        {
            entity = _dbSet.Attach(entity).Entity;
        }
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

        public virtual Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
            _dbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }
    }
}
