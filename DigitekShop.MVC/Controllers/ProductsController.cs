using Microsoft.AspNetCore.Mvc;
using DigitekShop.Infrastructure.ExternalServices;
using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.Features.Products.Commands.CreateProduct;
using DigitekShop.Application.Features.Products.Commands.UpdateProduct;
using DigitekShop.Application.Features.Products.Queries.GetProducts;
using DigitekShop.Application.Features.Products.Queries.GetProduct;

namespace DigitekShop.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IExternalService _externalService;

        public ProductsController(IExternalService externalService)
        {
            _externalService = externalService;
        }

        public async Task<IActionResult> Index(GetProductsQuery query = null)
        {
            try
            {
                var products = await _externalService.GetAsync<PagedResultDto<ProductListDto>>("api/products", query);
                return View(products);
            }
            catch (Exception ex)
            {
                // Log the error
                ModelState.AddModelError("", "خطا در دریافت محصولات");
                return View(new PagedResultDto<ProductListDto> { Items = new List<ProductListDto>() });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _externalService.GetAsync<ProductDto>($"api/products/{id}");
                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "خطا در دریافت محصول");
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _externalService.PostAsync<CreateProductCommand, ProductDto>("api/products", command);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "خطا در ایجاد محصول");
                }
            }
            return View(command);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _externalService.GetAsync<ProductDto>($"api/products/{id}");
                var command = new UpdateProductCommand
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity
                };
                return View(command);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "خطا در دریافت محصول");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _externalService.PutAsync<UpdateProductCommand, ProductDto>($"api/products/{command.Id}", command);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "خطا در بروزرسانی محصول");
                }
            }
            return View(command);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _externalService.GetAsync<ProductDto>($"api/products/{id}");
                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "خطا در دریافت محصول");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _externalService.DeleteAsync($"api/products/{id}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "خطا در حذف محصول");
                return RedirectToAction(nameof(Index));
            }
        }
    }
} 