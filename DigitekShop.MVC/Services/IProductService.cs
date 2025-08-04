using DigitekShop.MVC.Models;

namespace DigitekShop.MVC.Services
{
    public interface IProductService
    {
        Task<PagedProductViewModel> GetProductsAsync(int pageNumber = 1, int pageSize = 10);
        Task<ProductViewModel?> GetProductByIdAsync(int id);
        Task<PagedProductViewModel> SearchProductsAsync(string searchTerm, int pageNumber = 1, int pageSize = 10);
        Task<PagedProductViewModel> GetProductsByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10);
        Task<List<ProductViewModel>> GetTopSellingProductsAsync(int count = 10);
        Task<List<ProductViewModel>> GetNewArrivalsAsync(int count = 10);
    }
} 