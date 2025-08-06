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
    //public class RolesController : ControllerBase
    //{
    //    private readonly IRoleService _roleService;

    //    public RolesController(IRoleService roleService)
    //    {
    //        _roleService = roleService;
    //    }

    //    #region Role Management

    //    /// <summary>
    //    /// دریافت لیست نقش‌ها
    //    /// </summary>
    //    [HttpGet]
    //    public async Task<ActionResult<ApiResponseDto<PagedResultDto<RoleDto>>>> GetRoles(
    //        [FromQuery] int page = 1, 
    //        [FromQuery] int pageSize = 20,
    //        [FromQuery] string? searchTerm = null)
    //    {
    //        try
    //        {
    //            var result = await _roleService.GetRolesAsync(page, pageSize, searchTerm);
    //            return Ok(ApiResponseDto<PagedResultDto<RoleDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<PagedResultDto<RoleDto>>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// دریافت اطلاعات نقش
    //    /// </summary>
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<ApiResponseDto<RoleDto>>> GetRole(string id)
    //    {
    //        try
    //        {
    //            var result = await _roleService.GetRoleByIdAsync(id);
    //            return Ok(ApiResponseDto<RoleDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<RoleDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// ایجاد نقش جدید
    //    /// </summary>
    //    [HttpPost]
    //    public async Task<ActionResult<ApiResponseDto<RoleDto>>> CreateRole([FromBody] CreateRoleDto createRoleDto)
    //    {
    //        try
    //        {
    //            var result = await _roleService.CreateRoleAsync(createRoleDto);
    //            return Ok(ApiResponseDto<RoleDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<RoleDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// به‌روزرسانی نقش
    //    /// </summary>
    //    [HttpPut("{id}")]
    //    public async Task<ActionResult<ApiResponseDto<RoleDto>>> UpdateRole(string id, [FromBody] UpdateRoleDto updateRoleDto)
    //    {
    //        try
    //        {
    //            updateRoleDto.Id = id;
    //            var result = await _roleService.UpdateRoleAsync(updateRoleDto);
    //            return Ok(ApiResponseDto<RoleDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<RoleDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// حذف نقش
    //    /// </summary>
    //    [HttpDelete("{id}")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteRole(string id)
    //    {
    //        try
    //        {
    //            var result = await _roleService.DeleteRoleAsync(id);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region Role Permissions

    //    /// <summary>
    //    /// دریافت مجوزهای نقش
    //    /// </summary>
    //    [HttpGet("{id}/permissions")]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<string>>>> GetRolePermissions(string id)
    //    {
    //        try
    //        {
    //            var result = await _roleService.GetRolePermissionsAsync(id);
    //            return Ok(ApiResponseDto<IEnumerable<string>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<string>>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// اضافه کردن مجوز به نقش
    //    /// </summary>
    //    [HttpPost("{id}/permissions")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> AddPermissionToRole(string id, [FromBody] string permissionName)
    //    {
    //        try
    //        {
    //            var result = await _roleService.AddPermissionToRoleAsync(id, permissionName);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// حذف مجوز از نقش
    //    /// </summary>
    //    [HttpDelete("{id}/permissions/{permissionName}")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> RemovePermissionFromRole(string id, string permissionName)
    //    {
    //        try
    //        {
    //            var result = await _roleService.RemovePermissionFromRoleAsync(id, permissionName);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region Role Users

    //    /// <summary>
    //    /// دریافت کاربران نقش
    //    /// </summary>
    //    [HttpGet("{id}/users")]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<UserDto>>>> GetRoleUsers(string id)
    //    {
    //        try
    //        {
    //            var result = await _roleService.GetRoleUsersAsync(id);
    //            return Ok(ApiResponseDto<IEnumerable<UserDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<UserDto>>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region Analytics

    //    /// <summary>
    //    /// دریافت آمار نقش‌ها
    //    /// </summary>
    //    [HttpGet("analytics")]
    //    public async Task<ActionResult<ApiResponseDto<RoleAnalyticsDto>>> GetRoleAnalytics()
    //    {
    //        try
    //        {
    //            var result = await _roleService.GetRoleAnalyticsAsync();
    //            return Ok(ApiResponseDto<RoleAnalyticsDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<RoleAnalyticsDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// دریافت فعالیت‌های نقش
    //    /// </summary>
    //    [HttpGet("{id}/activities")]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<RoleActivityDto>>>> GetRoleActivities(string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    //    {
    //        try
    //        {
    //            var result = await _roleService.GetRoleActivitiesAsync(id, page, pageSize);
    //            return Ok(ApiResponseDto<IEnumerable<RoleActivityDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<RoleActivityDto>>.Error(ex.Message));
    //        }
    //    }

    //    #endregion
    //}
} 