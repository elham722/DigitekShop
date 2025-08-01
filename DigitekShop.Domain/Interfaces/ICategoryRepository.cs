using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetMainCategoriesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetHierarchyAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<int> GetProductCountAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<int> GetSubCategoryCountAsync(int categoryId, CancellationToken cancellationToken = default);
    }
} 