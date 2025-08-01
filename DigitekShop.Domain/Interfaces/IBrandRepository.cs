using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.Interfaces
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<Brand> GetBySlugAsync(string slug, CancellationToken cancellationToken=default);
        Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Brand>> GetByCountryAsync(string country, CancellationToken cancellationToken = default);
        Task<IEnumerable<Brand>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<int> GetProductCountAsync(int brandId, CancellationToken cancellationToken = default);
    }
} 