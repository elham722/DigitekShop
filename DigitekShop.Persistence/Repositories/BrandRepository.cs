using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.Repositories
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly DigitekShopDBContext _context;

        public BrandRepository(DigitekShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Brands
                .AnyAsync(b => b.Slug == slug, cancellationToken);
        }

        public async Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Brands
                .Where(b => b.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Brand>> GetByCountryAsync(string country, CancellationToken cancellationToken = default)
        {
            return await _context.Brands
                .Where(b => b.Country == country)
                .ToListAsync(cancellationToken);
        }

        public async Task<Brand> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Brands
                .FirstOrDefaultAsync(b => b.Slug == slug, cancellationToken);
        }

        public async Task<int> GetProductCountAsync(int brandId, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .CountAsync(p => p.BrandId == brandId, cancellationToken);
        }

        public async Task<IEnumerable<Brand>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            return await _context.Brands
                .Where(b => EF.Functions.Like(b.Name.Value, $"%{searchTerm}%"))
                .ToListAsync(cancellationToken);

        }
    }

}