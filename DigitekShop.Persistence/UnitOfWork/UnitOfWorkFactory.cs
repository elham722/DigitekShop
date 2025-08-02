using DigitekShop.Domain.Interfaces;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.UnitOfWork
{
    /// <summary>
    /// Factory implementation for creating UnitOfWork instances
    /// </summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory<DigitekShopDBContext> _contextFactory;

        public UnitOfWorkFactory(IDbContextFactory<DigitekShopDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IUnitOfWork Create()
        {
            var context = _contextFactory.CreateDbContext();
            return new UnitOfWork(context);
        }

        public async Task<IUnitOfWork> CreateWithTransactionAsync(CancellationToken cancellationToken = default)
        {
            var unitOfWork = Create();
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            return unitOfWork;
        }
    }
} 