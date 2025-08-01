using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DigitekShopDBContext _context;

        public CategoryRepository(DigitekShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(c => c.Slug == slug, cancellationToken);
        }

        public async Task<Category> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Slug == slug, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Where(c => c.Type == type)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetMainCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == parentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetSubCategoryCountAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .CountAsync(c => c.ParentCategoryId == categoryId, cancellationToken);
        }

        public async Task<int> GetProductCountAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .CountAsync(p => p.CategoryId == categoryId, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetHierarchyAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .ThenInclude(sc => sc.SubCategories)
                .ToListAsync(cancellationToken);
        }
    }

}