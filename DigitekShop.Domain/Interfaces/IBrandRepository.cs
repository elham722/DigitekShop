using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.Interfaces
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<Brand> GetBySlugAsync(string slug);
        Task<IEnumerable<Brand>> GetActiveBrandsAsync();
        Task<IEnumerable<Brand>> GetByCountryAsync(string country);
        Task<IEnumerable<Brand>> SearchByNameAsync(string searchTerm);
        Task<bool> ExistsBySlugAsync(string slug);
        Task<int> GetProductCountAsync(int brandId);
    }
} 