using DigitekShop.MVC.Models;
using DigitekShop.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitekShop.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 12)
        {
            try
            {
                var products = await _productService.GetProductsAsync(pageNumber, pageSize);
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products");
                return View(new PagedProductViewModel());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                
                if (product == null)
                {
                    return NotFound();
                }
                
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product details for ID: {ProductId}", id);
                return NotFound();
            }
        }

        public async Task<IActionResult> Search(string searchTerm, int pageNumber = 1, int pageSize = 12)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return RedirectToAction(nameof(Index));
                }

                var products = await _productService.SearchProductsAsync(searchTerm, pageNumber, pageSize);
                ViewBag.SearchTerm = searchTerm;
                
                return View("Index", products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching products");
                return View("Index", new PagedProductViewModel());
            }
        }

        public async Task<IActionResult> Category(int categoryId, int pageNumber = 1, int pageSize = 12)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAsync(categoryId, pageNumber, pageSize);
                ViewBag.CategoryId = categoryId;
                
                return View("Index", products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products by category: {CategoryId}", categoryId);
                return View("Index", new PagedProductViewModel());
            }
        }

        public async Task<IActionResult> TopSelling()
        {
            try
            {
                var products = await _productService.GetTopSellingProductsAsync(8);
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting top selling products");
                return View(new List<ProductViewModel>());
            }
        }

        public async Task<IActionResult> NewArrivals()
        {
            try
            {
                var products = await _productService.GetNewArrivalsAsync(8);
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting new arrivals");
                return View(new List<ProductViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdvancedSearch(ProductSearchViewModel searchModel)
        {
            try
            {
                if (searchModel.PageNumber <= 0)
                    searchModel.PageNumber = 1;

                if (searchModel.PageSize <= 0)
                    searchModel.PageSize = 12;

                PagedProductViewModel products;

                if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
                {
                    products = await _productService.SearchProductsAsync(
                        searchModel.SearchTerm, 
                        searchModel.PageNumber, 
                        searchModel.PageSize);
                }
                else if (searchModel.CategoryId.HasValue)
                {
                    products = await _productService.GetProductsByCategoryAsync(
                        searchModel.CategoryId.Value, 
                        searchModel.PageNumber, 
                        searchModel.PageSize);
                }
                else
                {
                    products = await _productService.GetProductsAsync(
                        searchModel.PageNumber, 
                        searchModel.PageSize);
                }

                ViewBag.SearchModel = searchModel;
                return View("Index", products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while performing advanced search");
                return View("Index", new PagedProductViewModel());
            }
        }
    }
} 