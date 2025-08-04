using DigitekShop.MVC.Models;
using DigitekShop.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DigitekShop.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get some featured products for the home page
                var featuredProducts = await _productService.GetTopSellingProductsAsync(6);
                ViewBag.FeaturedProducts = featuredProducts;
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading home page");
                ViewBag.FeaturedProducts = new List<ProductViewModel>();
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
