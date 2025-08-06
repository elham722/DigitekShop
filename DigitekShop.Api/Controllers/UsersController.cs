using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DigitekShop.Application.DTOs.Identity;
using DigitekShop.Application.Interfaces.Identity;
using DigitekShop.Application.DTOs.Common;

namespace DigitekShop.Api.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //[Authorize]
    //public class UsersController : ControllerBase
    //{
    //    private readonly IUserService _userService;

    //    public UsersController(IUserService userService)
    //    {
    //        _userService = userService;
    //    }

    //    #region User Management

    //    /// <summary>
    //    /// دریافت لیست کاربران
    //    /// </summary>
    //    [HttpGet]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<PagedResultDto<UserDto>>>> GetUsers(
    //        [FromQuery] int page = 1, 
    //        [FromQuery] int pageSize = 20,
    //        [FromQuery] string? searchTerm = null,
    //        [FromQuery] string? roleFilter = null,
    //        [FromQuery] bool? isActive = null)
    //    {
    //        try
    //        {
    //            var result = await _userService.GetUsersAsync(page, pageSize, searchTerm, roleFilter, isActive);
    //            return Ok(ApiResponseDto<PagedResultDto<UserDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<PagedResultDto<UserDto>>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// دریافت اطلاعات کاربر
    //    /// </summary>
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<ApiResponseDto<UserDto>>> GetUser(string id)
    //    {
    //        try
    //        {
    //            var result = await _userService.GetUserByIdAsync(id);
    //            return Ok(ApiResponseDto<UserDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<UserDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// ایجاد کاربر جدید
    //    /// </summary>
    //    [HttpPost]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<UserDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
    //    {
    //        try
    //        {
    //            var result = await _userService.CreateUserAsync(createUserDto);
    //            return Ok(ApiResponseDto<UserDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<UserDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// به‌روزرسانی اطلاعات کاربر
    //    /// </summary>
    //    [HttpPut("{id}")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<UserDto>>> UpdateUser(string id, [FromBody] UpdateProfileDto updateProfileDto)
    //    {
    //        try
    //        {
    //            var result = await _userService.UpdateUserAsync(id, updateProfileDto);
    //            return Ok(ApiResponseDto<UserDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<UserDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// حذف کاربر
    //    /// </summary>
    //    [HttpDelete("{id}")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteUser(string id)
    //    {
    //        try
    //        {
    //            var result = await _userService.DeleteUserAsync(id);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region User Profile

    //    /// <summary>
    //    /// دریافت پروفایل کاربر جاری
    //    /// </summary>
    //    [HttpGet("profile")]
    //    public async Task<ActionResult<ApiResponseDto<UserDto>>> GetCurrentUserProfile()
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<UserDto>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _userService.GetUserByIdAsync(userId);
    //            return Ok(ApiResponseDto<UserDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<UserDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// به‌روزرسانی پروفایل کاربر جاری
    //    /// </summary>
    //    [HttpPut("profile")]
    //    public async Task<ActionResult<ApiResponseDto<UserDto>>> UpdateCurrentUserProfile([FromBody] UpdateProfileDto updateProfileDto)
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<UserDto>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _userService.UpdateUserAsync(userId, updateProfileDto);
    //            return Ok(ApiResponseDto<UserDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<UserDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// به‌روزرسانی اطلاعات تماس
    //    /// </summary>
    //    [HttpPut("contact-info")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> UpdateContactInfo([FromBody] UpdateContactInfoDto updateContactInfoDto)
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<bool>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _userService.UpdateContactInfoAsync(userId, updateContactInfoDto);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region User Status Management

    //    /// <summary>
    //    /// فعال‌سازی کاربر
    //    /// </summary>
    //    [HttpPost("{id}/activate")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> ActivateUser(string id)
    //    {
    //        try
    //        {
    //            var result = await _userService.ActivateUserAsync(id);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// غیرفعال‌سازی کاربر
    //    /// </summary>
    //    [HttpPost("{id}/deactivate")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> DeactivateUser(string id)
    //    {
    //        try
    //        {
    //            var result = await _userService.DeactivateUserAsync(id);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// قفل کردن حساب کاربر
    //    /// </summary>
    //    [HttpPost("{id}/lock")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> LockUser(string id, [FromQuery] int lockoutMinutes = 30)
    //    {
    //        try
    //        {
    //            var result = await _userService.LockUserAsync(id, TimeSpan.FromMinutes(lockoutMinutes));
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// باز کردن قفل حساب کاربر
    //    /// </summary>
    //    [HttpPost("{id}/unlock")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> UnlockUser(string id)
    //    {
    //        try
    //        {
    //            var result = await _userService.UnlockUserAsync(id);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region User Roles

    //    /// <summary>
    //    /// دریافت نقش‌های کاربر
    //    /// </summary>
    //    [HttpGet("{id}/roles")]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<string>>>> GetUserRoles(string id)
    //    {
    //        try
    //        {
    //            var result = await _userService.GetUserRolesAsync(id);
    //            return Ok(ApiResponseDto<IEnumerable<string>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<string>>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// اضافه کردن نقش به کاربر
    //    /// </summary>
    //    [HttpPost("{id}/roles")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> AddUserToRole(string id, [FromBody] string roleName)
    //    {
    //        try
    //        {
    //            var result = await _userService.AddUserToRoleAsync(id, roleName);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// حذف نقش از کاربر
    //    /// </summary>
    //    [HttpDelete("{id}/roles/{roleName}")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> RemoveUserFromRole(string id, string roleName)
    //    {
    //        try
    //        {
    //            var result = await _userService.RemoveUserFromRoleAsync(id, roleName);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region Analytics

    //    /// <summary>
    //    /// دریافت آمار کاربران
    //    /// </summary>
    //    [HttpGet("analytics")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<UserAnalyticsDto>>> GetUserAnalytics()
    //    {
    //        try
    //        {
    //            var result = await _userService.GetUserAnalyticsAsync();
    //            return Ok(ApiResponseDto<UserAnalyticsDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<UserAnalyticsDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// دریافت فعالیت‌های کاربر
    //    /// </summary>
    //    [HttpGet("{id}/activities")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<UserActivityDto>>>> GetUserActivities(string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    //    {
    //        try
    //        {
    //            var result = await _userService.GetUserActivitiesAsync(id, page, pageSize);
    //            return Ok(ApiResponseDto<IEnumerable<UserActivityDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<UserActivityDto>>.Error(ex.Message));
    //        }
    //    }

    //    #endregion
    }
 