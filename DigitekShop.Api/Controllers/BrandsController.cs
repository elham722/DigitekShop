using Microsoft.AspNetCore.Mvc;
using DigitekShop.Application.DTOs.Brand;
using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Responses;
using DigitekShop.Api.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitekShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CorsPolicy]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BrandsController> _logger;

        public BrandsController(IMediator mediator, ILogger<BrandsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// دریافت لیست برندها
        /// </summary>
        /// <param name="pageNumber">شماره صفحه</param>
        /// <param name="pageSize">اندازه صفحه</param>
        /// <param name="searchTerm">عبارت جستجو</param>
        /// <param name="sortBy">مرتب‌سازی بر اساس</param>
        /// <param name="isAscending">مرتب‌سازی صعودی</param>
        /// <returns>لیست برندها</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<BrandDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetBrands(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isAscending = true)
        {
            try
            {
                _logger.LogInformation("Getting brands with page {PageNumber}, size {PageSize}", pageNumber, pageSize);
                
                // در اینجا باید یک کوئری برای دریافت برندها ایجاد شود
                // فعلاً به صورت موقت یک لیست خالی برمی‌گردانیم
                var brands = new List<BrandDto>();
                var result = new PagedResultDto<BrandDto>
                {
                    Items = brands,
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                
                _logger.LogInformation("Retrieved {Count} brands", brands.Count);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting brands");
                return StatusCode(500, new ErrorResponse("خطا در دریافت لیست برندها"));
            }
        }

        /// <summary>
        /// دریافت برند با شناسه
        /// </summary>
        /// <param name="id">شناسه برند</param>
        /// <returns>اطلاعات برند</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BrandDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetBrandById(int id)
        {
            try
            {
                _logger.LogInformation("Getting brand with ID: {BrandId}", id);
                
                // در اینجا باید یک کوئری برای دریافت برند با شناسه ایجاد شود
                // فعلاً به صورت موقت null برمی‌گردانیم
                BrandDto brand = null;
                
                if (brand == null)
                {
                    _logger.LogWarning("Brand not found with ID: {BrandId}", id);
                    return NotFound(new ErrorResponse("برند مورد نظر یافت نشد"));
                }
                
                _logger.LogInformation("Retrieved brand: {BrandName}", brand.Name);
                
                return Ok(brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting brand with ID: {BrandId}", id);
                return StatusCode(500, new ErrorResponse("خطا در دریافت اطلاعات برند"));
            }
        }

        /// <summary>
        /// ایجاد برند جدید
        /// </summary>
        /// <param name="createBrandDto">اطلاعات برند جدید</param>
        /// <returns>برند ایجاد شده</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<BrandDto>), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDto createBrandDto)
        {
            try
            {
                _logger.LogInformation("Creating new brand: {BrandName}", createBrandDto.Name);
                
                // در اینجا باید یک کامند برای ایجاد برند ایجاد شود
                // فعلاً به صورت موقت یک برند جدید با شناسه 1 برمی‌گردانیم
                var brand = new BrandDto
                {
                    Id = 1,
                    Name = createBrandDto.Name,
                    Description = createBrandDto.Description,
                    LogoUrl = createBrandDto.LogoUrl,
                    Website = createBrandDto.Website,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                
                _logger.LogInformation("Brand created successfully with ID: {BrandId}", brand.Id);
                
                return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, 
                    new SuccessResponse<BrandDto>(brand, "برند با موفقیت ایجاد شد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating brand");
                return StatusCode(500, new ErrorResponse("خطا در ایجاد برند"));
            }
        }

        /// <summary>
        /// به‌روزرسانی برند
        /// </summary>
        /// <param name="id">شناسه برند</param>
        /// <param name="updateBrandDto">اطلاعات جدید برند</param>
        /// <returns>برند به‌روزرسانی شده</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(SuccessResponse<BrandDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] UpdateBrandDto updateBrandDto)
        {
            try
            {
                if (id != updateBrandDto.Id)
                {
                    _logger.LogWarning("ID mismatch in URL ({UrlId}) and body ({BodyId})", id, updateBrandDto.Id);
                    return BadRequest(new ErrorResponse("شناسه در URL و بدنه درخواست مطابقت ندارد"));
                }
                
                _logger.LogInformation("Updating brand with ID: {BrandId}", id);
                
                // در اینجا باید یک کامند برای به‌روزرسانی برند ایجاد شود
                // فعلاً به صورت موقت یک برند به‌روزرسانی شده برمی‌گردانیم
                var brand = new BrandDto
                {
                    Id = id,
                    Name = updateBrandDto.Name ?? "برند",
                    Description = updateBrandDto.Description,
                    LogoUrl = updateBrandDto.LogoUrl,
                    Website = updateBrandDto.Website,
                    IsActive = true,
                    UpdatedAt = DateTime.UtcNow
                };
                
                _logger.LogInformation("Brand updated successfully: {BrandName}", brand.Name);
                
                return Ok(new SuccessResponse<BrandDto>(brand, "برند با موفقیت به‌روزرسانی شد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating brand with ID: {BrandId}", id);
                return StatusCode(500, new ErrorResponse("خطا در به‌روزرسانی برند"));
            }
        }

        /// <summary>
        /// حذف برند
        /// </summary>
        /// <param name="id">شناسه برند</param>
        /// <returns>نتیجه حذف</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                _logger.LogInformation("Deleting brand with ID: {BrandId}", id);
                
                // در اینجا باید یک کامند برای حذف برند ایجاد شود
                // فعلاً به صورت موقت فرض می‌کنیم که برند با موفقیت حذف شده است
                
                _logger.LogInformation("Brand deleted successfully with ID: {BrandId}", id);
                
                return Ok(new SuccessResponse<object>("برند با موفقیت حذف شد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting brand with ID: {BrandId}", id);
                return StatusCode(500, new ErrorResponse("خطا در حذف برند"));
            }
        }

        /// <summary>
        /// دریافت برندهای محبوب
        /// </summary>
        /// <param name="count">تعداد برندها</param>
        /// <returns>لیست برندهای محبوب</returns>
        [HttpGet("popular")]
        [ProducesResponseType(typeof(IEnumerable<BrandDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetPopularBrands([FromQuery] int count = 10)
        {
            try
            {
                _logger.LogInformation("Getting {Count} popular brands", count);
                
                // در اینجا باید یک کوئری برای دریافت برندهای محبوب ایجاد شود
                // فعلاً به صورت موقت یک لیست خالی برمی‌گردانیم
                var brands = new List<BrandDto>();
                
                _logger.LogInformation("Retrieved {Count} popular brands", brands.Count);
                
                return Ok(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting popular brands");
                return StatusCode(500, new ErrorResponse("خطا در دریافت لیست برندهای محبوب"));
            }
        }
    }
}