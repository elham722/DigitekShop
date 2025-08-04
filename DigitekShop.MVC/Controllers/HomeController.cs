using Microsoft.AspNetCore.Mvc;
using DigitekShop.Infrastructure.ExternalServices;
using DigitekShop.Application.DTOs.Product;
using System.Diagnostics;
using DigitekShop.MVC.Models;

namespace DigitekShop.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IExternalService _externalService;

        public HomeController(ILogger<HomeController> logger, IExternalService externalService)
        {
            _logger = logger;
            _externalService = externalService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get some featured products for the home page
                var featuredProducts = await _externalService.GetAsync<List<ProductDto>>("api/products/top-selling?count=6");
                ViewBag.FeaturedProducts = featuredProducts;
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading home page");
                ViewBag.FeaturedProducts = new List<ProductDto>();
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
