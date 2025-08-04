using Microsoft.AspNetCore.Mvc;
using DigitekShop.Application.Features.Products.Queries.GetProducts;
using DigitekShop.Application.Features.Products.Queries.GetProduct;
using DigitekShop.Application.Features.Products.Commands.CreateProduct;
using DigitekShop.Application.Features.Products.Commands.UpdateProduct;
using DigitekShop.Application.Features.Products.Commands.DeleteProduct;
using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Responses;
using DigitekShop.Api.Attributes;
using DigitekShop.Application.Exceptions;
using DigitekShop.Domain.Exceptions;
using DigitekShop.Application.Extensions;

namespace DigitekShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CorsPolicy]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        private IQueryDispatcher QueryDispatcher => HttpContext.QueryDispatcher();
        private ICommandDispatcher CommandDispatcher => HttpContext.CommandDispatcher();

        /// <summary>
        /// دریافت لیست محصولات با قابلیت فیلتر و صفحه‌بندی
        /// </summary>
        /// <param name="query">پارامترهای فیلتر و صفحه‌بندی</param>
        /// <returns>لیست محصولات</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<ProductListDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
        {
            try
            {
                _logger.LogInformation("Getting products with filters: {@Filters}", query);
                
                var result = await QueryDispatcher.DispatchAsync(query);
                
                _logger.LogInformation("Retrieved {Count} products", result.Items?.Count() ?? 0);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products");
                return StatusCode(500, new ErrorResponse("خطا در دریافت لیست محصولات"));
            }
        }

        /// <summary>
        /// دریافت محصول بر اساس شناسه
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>جزئیات محصول</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation("Getting product with ID: {ProductId}", id);
                
                var query = new GetProductQuery { Id = id };
                var result = await QueryDispatcher.DispatchAsync(query);
                
                if (result == null || result.Data == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                    return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));
                }
                
                _logger.LogInformation("Retrieved product: {ProductName}", result.Data.Name);
                return Ok(result.Data);
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product with ID: {ProductId}", id);
                return StatusCode(500, new ErrorResponse("خطا در دریافت محصول"));
            }
        }

        /// <summary>
        /// ایجاد محصول جدید
        /// </summary>
        /// <param name="command">اطلاعات محصول جدید</param>
        /// <returns>محصول ایجاد شده</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            try
            {
                _logger.LogInformation("Creating new product: {@ProductData}", command);
                
                var result = await CommandDispatcher.DispatchAsync(command);
                
                if (result == null || result.Data == null)
                {
                    _logger.LogError("Failed to create product - no data returned");
                    return StatusCode(500, new ErrorResponse("خطا در ایجاد محصول"));
                }
                
                _logger.LogInformation("Product created successfully with ID: {ProductId}", result.Data.Id);
                
                return CreatedAtAction(nameof(GetProduct), new { id = result.Data.Id }, result.Data);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error while creating product: {Errors}", ex.Errors);
                return BadRequest(new ErrorResponse("خطا در اعتبارسنجی داده‌ها", ex.Errors));
            }
            catch (DuplicateEntityException ex)
            {
                _logger.LogWarning("Duplicate entity error: {Message}", ex.Message);
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch (CategoryNotFoundException ex)
            {
                _logger.LogWarning("Category not found: {CategoryId}", ex.CategoryId);
                return BadRequest(new ErrorResponse($"دسته‌بندی با شناسه {ex.CategoryId} یافت نشد"));
            }
            catch (BrandNotFoundException ex)
            {
                _logger.LogWarning("Brand not found: {BrandId}", ex.BrandId);
                return BadRequest(new ErrorResponse($"برند با شناسه {ex.BrandId} یافت نشد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                return StatusCode(500, new ErrorResponse("خطا در ایجاد محصول"));
            }
        }

        /// <summary>
        /// به‌روزرسانی محصول
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <param name="command">اطلاعات جدید محصول</param>
        /// <returns>محصول به‌روزرسانی شده</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    _logger.LogWarning("ID mismatch in URL ({UrlId}) and body ({BodyId})", id, command.Id);
                    return BadRequest(new ErrorResponse("شناسه در URL و بدنه درخواست مطابقت ندارد"));
                }
                
                _logger.LogInformation("Updating product with ID: {ProductId}", id);
                
                var result = await CommandDispatcher.DispatchAsync(command);
                
                if (result == null || result.Data == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for update", id);
                    return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));
                }
                
                _logger.LogInformation("Product updated successfully: {ProductName}", result.Data.Name);
                return Ok(result.Data);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error while updating product: {Errors}", ex.Errors);
                return BadRequest(new ErrorResponse("خطا در اعتبارسنجی داده‌ها", ex.Errors));
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogWarning("Product not found for update: {ProductId}", ex.ProductId);
                return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));
            }
            catch (DuplicateEntityException ex)
            {
                _logger.LogWarning("Duplicate entity error: {Message}", ex.Message);
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with ID: {ProductId}", id);
                return StatusCode(500, new ErrorResponse("خطا در به‌روزرسانی محصول"));
            }
        }

        /// <summary>
        /// حذف محصول
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>نتیجه حذف</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("Deleting product with ID: {ProductId}", id);

                var command = new DeleteProductCommand { Id = id };
                await CommandDispatcher.DispatchAsync(command);

                _logger.LogInformation("Product deleted successfully with ID: {ProductId}", id);
                return Ok(new SuccessResponse<object>("محصول با موفقیت حذف شد"));
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogWarning("Product not found for deletion: {ProductId}", ex.ProductId);
                return NotFound(new ErrorResponse("محصول مورد نظر یافت نشد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID: {ProductId}", id);
                return StatusCode(500, new ErrorResponse("خطا در حذف محصول"));
            }
        }

        /// <summary>
        /// دریافت محصولات بر اساس دسته‌بندی
        /// </summary>
        /// <param name="categoryId">شناسه دسته‌بندی</param>
        /// <param name="pageNumber">شماره صفحه</param>
        /// <param name="pageSize">اندازه صفحه</param>
        /// <returns>لیست محصولات دسته‌بندی</returns>
        [HttpGet("category/{categoryId:int}")]
        [ProducesResponseType(typeof(PagedResultDto<ProductListDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetProductsByCategory(int categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Getting products for category {CategoryId}", categoryId);
                
                var query = new GetProductsQuery
                {
                    CategoryId = categoryId,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                
                var result = await QueryDispatcher.DispatchAsync(query);
                
                _logger.LogInformation("Retrieved {Count} products for category {CategoryId}", 
                    result.Items?.Count() ?? 0, categoryId);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products for category {CategoryId}", categoryId);
                return StatusCode(500, new ErrorResponse("خطا در دریافت محصولات دسته‌بندی"));
            }
        }

        /// <summary>
        /// جستجو در محصولات
        /// </summary>
        /// <param name="searchTerm">عبارت جستجو</param>
        /// <param name="pageNumber">شماره صفحه</param>
        /// <param name="pageSize">اندازه صفحه</param>
        /// <returns>نتایج جستجو</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResultDto<ProductListDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    _logger.LogWarning("Empty search term provided");
                    return BadRequest(new ErrorResponse("عبارت جستجو نمی‌تواند خالی باشد"));
                }
                
                _logger.LogInformation("Searching products with term: {SearchTerm}", searchTerm);
                
                var query = new GetProductsQuery
                {
                    SearchTerm = searchTerm,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                
                var result = await QueryDispatcher.DispatchAsync(query);
                
                _logger.LogInformation("Found {Count} products for search term '{SearchTerm}'", 
                    result.Items?.Count() ?? 0, searchTerm);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching products with term: {SearchTerm}", searchTerm);
                return StatusCode(500, new ErrorResponse("خطا در جستجوی محصولات"));
            }
        }

        /// <summary>
        /// دریافت محصولات پرفروش
        /// </summary>
        /// <param name="count">تعداد محصولات</param>
        /// <returns>لیست محصولات پرفروش</returns>
        [HttpGet("top-selling")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] int count = 10)
        {
            try
            {
                _logger.LogInformation("Getting top {Count} selling products", count);
                
                var query = new GetProductsQuery
                {
                    PageSize = count,
                    SortBy = "SalesCount",
                    SortOrder = "Descending"
                };
                
                var result = await QueryDispatcher.DispatchAsync(query);
                
                _logger.LogInformation("Retrieved {Count} top selling products", result.Items?.Count() ?? 0);
                
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting top selling products");
                return StatusCode(500, new ErrorResponse("خطا در دریافت محصولات پرفروش"));
            }
        }

        /// <summary>
        /// دریافت محصولات جدید
        /// </summary>
        /// <param name="count">تعداد محصولات</param>
        /// <returns>لیست محصولات جدید</returns>
        [HttpGet("new-arrivals")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetNewArrivals([FromQuery] int count = 10)
        {
            try
            {
                _logger.LogInformation("Getting {Count} new arrival products", count);
                
                var query = new GetProductsQuery
                {
                    PageSize = count,
                    SortBy = "CreatedAt",
                    SortOrder = "Descending"
                };
                
                var result = await QueryDispatcher.DispatchAsync(query);
                
                _logger.LogInformation("Retrieved {Count} new arrival products", result.Items?.Count() ?? 0);
                
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting new arrival products");
                return StatusCode(500, new ErrorResponse("خطا در دریافت محصولات جدید"));
            }
        }
    }
} 