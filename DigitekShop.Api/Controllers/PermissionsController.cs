using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DigitekShop.Application.DTOs.Identity;
using DigitekShop.Application.Interfaces.Identity;
using DigitekShop.Application.DTOs.Common;

namespace DigitekShop.Api.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    //public class PermissionsController : ControllerBase
    //{
    //    private readonly IPermissionService _permissionService;

    //    public PermissionsController(IPermissionService permissionService)
    //    {
    //        _permissionService = permissionService;
    //    }

    //    #region Permission Management

    //    /// <summary>
    //    /// دریافت لیست مجوزها
    //    /// </summary>
    //    [HttpGet]
    //    public async Task<ActionResult<ApiResponseDto<PagedResultDto<PermissionDto>>>> GetPermissions(
    //        [FromQuery] int page = 1, 
    //        [FromQuery] int pageSize = 20,
    //        [FromQuery] string? searchTerm = null,
    //        [FromQuery] string? categoryFilter = null)
    //    {
    //        try
    //        {
    //            var result = await _permissionService.GetPermissionsAsync(page, pageSize, searchTerm, categoryFilter);
    //            return Ok(ApiResponseDto<PagedResultDto<PermissionDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<PagedResultDto<PermissionDto>>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// دریافت اطلاعات مجوز
    //    /// </summary>
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<ApiResponseDto<PermissionDto>>> GetPermission(string id)
    //    {
    //        try
    //        {
    //            var result = await _permissionService.GetPermissionByIdAsync(id);
    //            return Ok(ApiResponseDto<PermissionDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<PermissionDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// ایجاد مجوز جدید
    //    /// </summary>
    //    [HttpPost]
    //    public async Task<ActionResult<ApiResponseDto<PermissionDto>>> CreatePermission([FromBody] CreatePermissionDto createPermissionDto)
    //    {
    //        try
    //        {
    //            var result = await _permissionService.CreatePermissionAsync(createPermissionDto);
    //            return Ok(ApiResponseDto<PermissionDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<PermissionDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// به‌روزرسانی مجوز
    //    /// </summary>
    //    [HttpPut("{id}")]
    //    public async Task<ActionResult<ApiResponseDto<PermissionDto>>> UpdatePermission(string id, [FromBody] UpdatePermissionDto updatePermissionDto)
    //    {
    //        try
    //        {
    //            updatePermissionDto.Id = id;
    //            var result = await _permissionService.UpdatePermissionAsync(updatePermissionDto);
    //            return Ok(ApiResponseDto<PermissionDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<PermissionDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// حذف مجوز
    //    /// </summary>
    //    [HttpDelete("{id}")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> DeletePermission(string id)
    //    {
    //        try
    //        {
    //            var result = await _permissionService.DeletePermissionAsync(id);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region Permission Categories

    //    /// <summary>
    //    /// دریافت دسته‌بندی‌های مجوز
    //    /// </summary>
    //    [HttpGet("categories")]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<PermissionCategoryDto>>>> GetPermissionCategories()
    //    {
    //        try
    //        {
    //            var result = await _permissionService.GetPermissionCategoriesAsync();
    //            return Ok(ApiResponseDto<IEnumerable<PermissionCategoryDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<PermissionCategoryDto>>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// ایجاد دسته‌بندی مجوز جدید
    //    /// </summary>
    //    [HttpPost("categories")]
    //    public async Task<ActionResult<ApiResponseDto<PermissionCategoryDto>>> CreatePermissionCategory([FromBody] CreatePermissionCategoryDto createCategoryDto)
    //    {
    //        try
    //        {
    //            var result = await _permissionService.CreatePermissionCategoryAsync(createCategoryDto);
    //            return Ok(ApiResponseDto<PermissionCategoryDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<PermissionCategoryDto>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region Permission Roles

    //    /// <summary>
    //    /// دریافت نقش‌های مجوز
    //    /// </summary>
    //    [HttpGet("{id}/roles")]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<string>>>> GetPermissionRoles(string id)
    //    {
    //        try
    //        {
    //            var result = await _permissionService.GetPermissionRolesAsync(id);
    //            return Ok(ApiResponseDto<IEnumerable<string>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<string>>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region Analytics

    //    /// <summary>
    //    /// دریافت آمار مجوزها
    //    /// </summary>
    //    [HttpGet("analytics")]
    //    public async Task<ActionResult<ApiResponseDto<PermissionAnalyticsDto>>> GetPermissionAnalytics()
    //    {
    //        try
    //        {
    //            var result = await _permissionService.GetPermissionAnalyticsAsync();
    //            return Ok(ApiResponseDto<PermissionAnalyticsDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<PermissionAnalyticsDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// دریافت فعالیت‌های مجوز
    //    /// </summary>
    //    [HttpGet("{id}/activities")]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<PermissionActivityDto>>>> GetPermissionActivities(string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    //    {
    //        try
    //        {
    //            var result = await _permissionService.GetPermissionActivitiesAsync(id, page, pageSize);
    //            return Ok(ApiResponseDto<IEnumerable<PermissionActivityDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<PermissionActivityDto>>.Error(ex.Message));
    //        }
    //    }

    //    #endregion
    //}
} 