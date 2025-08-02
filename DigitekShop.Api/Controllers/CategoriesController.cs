using Microsoft.AspNetCore.Mvc;
using DigitekShop.Application.DTOs.Category;
using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Responses;
using DigitekShop.Api.Attributes;
using DigitekShop.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitekShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CorsPolicy]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// دریافت لیست دسته‌بندی‌ها
        /// </summary>
        /// <param name="type">نوع دسته‌بندی</param>
        /// <param name="parentId">شناسه دسته‌بندی والد</param>
        /// <returns>لیست دسته‌بندی‌ها</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetCategories(
            [FromQuery] CategoryType? type = null,
            [FromQuery] int? parentId = null)
        {
            try
            {
                _logger.LogInformation("Getting categories with type: {Type}, parentId: {ParentId}", type, parentId);
                
                // در اینجا باید یک کوئری برای دریافت دسته‌بندی‌ها ایجاد شود
                // فعلاً به صورت موقت یک لیست خالی برمی‌گردانیم
                var categories = new List<CategoryDto>();
                
                _logger.LogInformation("Retrieved {Count} categories", categories.Count);
                
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting categories");
                return StatusCode(500, new ErrorResponse("خطا در دریافت لیست دسته‌بندی‌ها"));
            }
        }

        /// <summary>
        /// دریافت دسته‌بندی با شناسه
        /// </summary>
        /// <param name="id">شناسه دسته‌بندی</param>
        /// <returns>اطلاعات دسته‌بندی</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CategoryDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                _logger.LogInformation("Getting category with ID: {CategoryId}", id);
                
                // در اینجا باید یک کوئری برای دریافت دسته‌بندی با شناسه ایجاد شود
                // فعلاً به صورت موقت null برمی‌گردانیم
                CategoryDto category = null;
                
                if (category == null)
                {
                    _logger.LogWarning("Category not found with ID: {CategoryId}", id);
                    return NotFound(new ErrorResponse("دسته‌بندی مورد نظر یافت نشد"));
                }
                
                _logger.LogInformation("Retrieved category: {CategoryName}", category.Name);
                
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting category with ID: {CategoryId}", id);
                return StatusCode(500, new ErrorResponse("خطا در دریافت اطلاعات دسته‌بندی"));
            }
        }

        /// <summary>
        /// ایجاد دسته‌بندی جدید
        /// </summary>
        /// <param name="createCategoryDto">اطلاعات دسته‌بندی جدید</param>
        /// <returns>دسته‌بندی ایجاد شده</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<CategoryDto>), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {
                _logger.LogInformation("Creating new category: {CategoryName}", createCategoryDto.Name);
                
                // در اینجا باید یک کامند برای ایجاد دسته‌بندی ایجاد شود
                // فعلاً به صورت موقت یک دسته‌بندی جدید با شناسه 1 برمی‌گردانیم
                var category = new CategoryDto
                {
                    Id = 1,
                    Name = createCategoryDto.Name,
                    Description = createCategoryDto.Description,
                    Type = createCategoryDto.Type,
                    ParentCategoryId = createCategoryDto.ParentCategoryId,
                    ImageUrl = createCategoryDto.ImageUrl,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                
                _logger.LogInformation("Category created successfully with ID: {CategoryId}", category.Id);
                
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, 
                    new SuccessResponse<CategoryDto>(category, "دسته‌بندی با موفقیت ایجاد شد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category");
                return StatusCode(500, new ErrorResponse("خطا در ایجاد دسته‌بندی"));
            }
        }

        /// <summary>
        /// به‌روزرسانی دسته‌بندی
        /// </summary>
        /// <param name="id">شناسه دسته‌بندی</param>
        /// <param name="updateCategoryDto">اطلاعات جدید دسته‌بندی</param>
        /// <returns>دسته‌بندی به‌روزرسانی شده</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(SuccessResponse<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                if (id != updateCategoryDto.Id)
                {
                    _logger.LogWarning("ID mismatch in URL ({UrlId}) and body ({BodyId})", id, updateCategoryDto.Id);
                    return BadRequest(new ErrorResponse("شناسه در URL و بدنه درخواست مطابقت ندارد"));
                }
                
                _logger.LogInformation("Updating category with ID: {CategoryId}", id);
                
                // در اینجا باید یک کامند برای به‌روزرسانی دسته‌بندی ایجاد شود
                // فعلاً به صورت موقت یک دسته‌بندی به‌روزرسانی شده برمی‌گردانیم
                var category = new CategoryDto
                {
                    Id = id,
                    Name = updateCategoryDto.Name ?? "دسته‌بندی",
                    Description = updateCategoryDto.Description,
                    Type = updateCategoryDto.Type ?? CategoryType.Main,
                    ParentCategoryId = updateCategoryDto.ParentCategoryId,
                    ImageUrl = updateCategoryDto.ImageUrl,
                    IsActive = true,
                    UpdatedAt = DateTime.UtcNow
                };
                
                _logger.LogInformation("Category updated successfully: {CategoryName}", category.Name);
                
                return Ok(new SuccessResponse<CategoryDto>(category, "دسته‌بندی با موفقیت به‌روزرسانی شد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with ID: {CategoryId}", id);
                return StatusCode(500, new ErrorResponse("خطا در به‌روزرسانی دسته‌بندی"));
            }
        }

        /// <summary>
        /// حذف دسته‌بندی
        /// </summary>
        /// <param name="id">شناسه دسته‌بندی</param>
        /// <returns>نتیجه حذف</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                _logger.LogInformation("Deleting category with ID: {CategoryId}", id);
                
                // در اینجا باید یک کامند برای حذف دسته‌بندی ایجاد شود
                // فعلاً به صورت موقت فرض می‌کنیم که دسته‌بندی با موفقیت حذف شده است
                
                _logger.LogInformation("Category deleted successfully with ID: {CategoryId}", id);
                
                return Ok(new SuccessResponse<object>("دسته‌بندی با موفقیت حذف شد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category with ID: {CategoryId}", id);
                return StatusCode(500, new ErrorResponse("خطا در حذف دسته‌بندی"));
            }
        }

        /// <summary>
        /// دریافت دسته‌بندی‌های اصلی
        /// </summary>
        /// <returns>لیست دسته‌بندی‌های اصلی</returns>
        [HttpGet("main")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetMainCategories()
        {
            try
            {
                _logger.LogInformation("Getting main categories");
                
                // در اینجا باید یک کوئری برای دریافت دسته‌بندی‌های اصلی ایجاد شود
                // فعلاً به صورت موقت یک لیست خالی برمی‌گردانیم
                var categories = new List<CategoryDto>();
                
                _logger.LogInformation("Retrieved {Count} main categories", categories.Count);
                
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting main categories");
                return StatusCode(500, new ErrorResponse("خطا در دریافت لیست دسته‌بندی‌های اصلی"));
            }
        }

        /// <summary>
        /// دریافت زیردسته‌های یک دسته‌بندی
        /// </summary>
        /// <param name="id">شناسه دسته‌بندی والد</param>
        /// <returns>لیست زیردسته‌ها</returns>
        [HttpGet("{id:int}/subcategories")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetSubCategories(int id)
        {
            try
            {
                _logger.LogInformation("Getting subcategories for category ID: {CategoryId}", id);
                
                // در اینجا باید یک کوئری برای دریافت زیردسته‌های یک دسته‌بندی ایجاد شود
                // فعلاً به صورت موقت یک لیست خالی برمی‌گردانیم
                var categories = new List<CategoryDto>();
                
                _logger.LogInformation("Retrieved {Count} subcategories for category ID: {CategoryId}", 
                    categories.Count, id);
                
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting subcategories for category ID: {CategoryId}", id);
                return StatusCode(500, new ErrorResponse("خطا در دریافت لیست زیردسته‌ها"));
            }
        }
    }
}