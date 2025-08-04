using DigitekShop.MVC.Models;
using Newtonsoft.Json;
using System.Text;

namespace DigitekShop.MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;
        private readonly string _apiBaseUrl;

        public ProductService(HttpClient httpClient, ILogger<ProductService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001";
        }

        public async Task<PagedProductViewModel> GetProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var url = $"{_apiBaseUrl}/api/products?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiPagedResponse>(content);
                    
                    return new PagedProductViewModel
                    {
                        Items = apiResponse?.Items?.Select(MapToViewModel).ToList() ?? new List<ProductViewModel>(),
                        TotalCount = apiResponse?.TotalCount ?? 0,
                        PageNumber = apiResponse?.PageNumber ?? pageNumber,
                        PageSize = apiResponse?.PageSize ?? pageSize
                    };
                }
                
                _logger.LogError("Failed to get products. Status: {StatusCode}", response.StatusCode);
                return new PagedProductViewModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products");
                return new PagedProductViewModel();
            }
        }

        public async Task<ProductViewModel?> GetProductByIdAsync(int id)
        {
            try
            {
                var url = $"{_apiBaseUrl}/api/products/{id}";
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiProduct = JsonConvert.DeserializeObject<ApiProductResponse>(content);
                    return MapToViewModel(apiProduct);
                }
                
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product with ID: {ProductId}", id);
                return null;
            }
        }

        public async Task<PagedProductViewModel> SearchProductsAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var url = $"{_apiBaseUrl}/api/products/search?searchTerm={Uri.EscapeDataString(searchTerm)}&pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiPagedResponse>(content);
                    
                    return new PagedProductViewModel
                    {
                        Items = apiResponse?.Items?.Select(MapToViewModel).ToList() ?? new List<ProductViewModel>(),
                        TotalCount = apiResponse?.TotalCount ?? 0,
                        PageNumber = apiResponse?.PageNumber ?? pageNumber,
                        PageSize = apiResponse?.PageSize ?? pageSize
                    };
                }
                
                _logger.LogError("Failed to search products. Status: {StatusCode}", response.StatusCode);
                return new PagedProductViewModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching products");
                return new PagedProductViewModel();
            }
        }

        public async Task<PagedProductViewModel> GetProductsByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var url = $"{_apiBaseUrl}/api/products/category/{categoryId}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiPagedResponse>(content);
                    
                    return new PagedProductViewModel
                    {
                        Items = apiResponse?.Items?.Select(MapToViewModel).ToList() ?? new List<ProductViewModel>(),
                        TotalCount = apiResponse?.TotalCount ?? 0,
                        PageNumber = apiResponse?.PageNumber ?? pageNumber,
                        PageSize = apiResponse?.PageSize ?? pageSize
                    };
                }
                
                _logger.LogError("Failed to get products by category. Status: {StatusCode}", response.StatusCode);
                return new PagedProductViewModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products by category");
                return new PagedProductViewModel();
            }
        }

        public async Task<List<ProductViewModel>> GetTopSellingProductsAsync(int count = 10)
        {
            try
            {
                var url = $"{_apiBaseUrl}/api/products/top-selling?count={count}";
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiProducts = JsonConvert.DeserializeObject<List<ApiProductResponse>>(content);
                    return apiProducts?.Select(MapToViewModel).ToList() ?? new List<ProductViewModel>();
                }
                
                _logger.LogError("Failed to get top selling products. Status: {StatusCode}", response.StatusCode);
                return new List<ProductViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting top selling products");
                return new List<ProductViewModel>();
            }
        }

        public async Task<List<ProductViewModel>> GetNewArrivalsAsync(int count = 10)
        {
            try
            {
                var url = $"{_apiBaseUrl}/api/products/new-arrivals?count={count}";
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiProducts = JsonConvert.DeserializeObject<List<ApiProductResponse>>(content);
                    return apiProducts?.Select(MapToViewModel).ToList() ?? new List<ProductViewModel>();
                }
                
                _logger.LogError("Failed to get new arrivals. Status: {StatusCode}", response.StatusCode);
                return new List<ProductViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting new arrivals");
                return new List<ProductViewModel>();
            }
        }

        private static ProductViewModel MapToViewModel(ApiProductResponse? apiProduct)
        {
            if (apiProduct == null) return new ProductViewModel();
            
            return new ProductViewModel
            {
                Id = apiProduct.Id,
                Name = apiProduct.Name ?? string.Empty,
                Description = apiProduct.Description ?? string.Empty,
                Price = apiProduct.Price,
                Currency = apiProduct.Currency ?? string.Empty,
                StockQuantity = apiProduct.StockQuantity,
                SKU = apiProduct.SKU ?? string.Empty,
                Status = apiProduct.Status ?? string.Empty,
                CategoryId = apiProduct.CategoryId,
                CategoryName = apiProduct.CategoryName ?? string.Empty,
                BrandId = apiProduct.BrandId,
                BrandName = apiProduct.BrandName ?? string.Empty,
                Model = apiProduct.Model ?? string.Empty,
                Weight = apiProduct.Weight,
                CreatedAt = apiProduct.CreatedAt,
                UpdatedAt = apiProduct.UpdatedAt,
                AverageRating = apiProduct.AverageRating,
                ReviewCount = apiProduct.ReviewCount,
                ImageUrl = apiProduct.ImageUrl ?? string.Empty
            };
        }

        // API Response Models
        private class ApiProductResponse
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public string? Currency { get; set; }
            public int StockQuantity { get; set; }
            public string? SKU { get; set; }
            public string? Status { get; set; }
            public int CategoryId { get; set; }
            public string? CategoryName { get; set; }
            public int? BrandId { get; set; }
            public string? BrandName { get; set; }
            public string? Model { get; set; }
            public decimal Weight { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public decimal AverageRating { get; set; }
            public int ReviewCount { get; set; }
            public string? ImageUrl { get; set; }
        }

        private class ApiPagedResponse
        {
            public List<ApiProductResponse>? Items { get; set; }
            public int TotalCount { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }
    }
} 