using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetBySlugAsync(string slug);
        Task<IEnumerable<Category>> GetMainCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
        Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type);
        Task<IEnumerable<Category>> GetHierarchyAsync();
        Task<bool> ExistsBySlugAsync(string slug);
        Task<int> GetProductCountAsync(int categoryId);
        Task<int> GetSubCategoryCountAsync(int categoryId);
    }
} 