using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DigitekShop.Application.DTOs.Identity;
using DigitekShop.Application.Interfaces.Identity;
using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.Responses;

namespace DigitekShop.Api.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class AuthController : ControllerBase
    //{
    //    private readonly IAuthService _authService;
    //    private readonly IUserService _userService;

    //    public AuthController(IAuthService authService, IUserService userService)
    //    {
    //        _authService = authService;
    //        _userService = userService;
    //    }

    //    #region Authentication

    //    /// <summary>
    //    /// ورود کاربر
    //    /// </summary>
    //    [HttpPost("login")]
    //    public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    //    {
    //        try
    //        {
    //            var result = await _authService.LoginAsync(loginDto);
    //            return Ok(ApiResponseDto<AuthResponseDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<AuthResponseDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// ورود با احراز هویت دو مرحله‌ای
    //    /// </summary>
    //    [HttpPost("login-2fa")]
    //    public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> LoginWithTwoFactor([FromBody] LoginWithTwoFactorDto loginDto)
    //    {
    //        try
    //        {
    //            var result = await _authService.LoginWithTwoFactorAsync(loginDto);
    //            return Ok(ApiResponseDto<AuthResponseDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<AuthResponseDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// ثبت‌نام کاربر جدید
    //    /// </summary>
    //    [HttpPost("register")]
    //    public async Task<ActionResult<ApiResponseDto<RegistrationResponseDto>>> Register([FromBody] RegisterUserDto registerDto)
    //    {
    //        try
    //        {
    //            var result = await _authService.RegisterAsync(registerDto);
    //            return Ok(ApiResponseDto<RegistrationResponseDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<RegistrationResponseDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// خروج کاربر
    //    /// </summary>
    //    [HttpPost("logout")]
    //    [Authorize]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> Logout()
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<bool>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _authService.LogoutAsync(userId);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// تغییر رمز عبور
    //    /// </summary>
    //    [HttpPost("change-password")]
    //    [Authorize]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<bool>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// فراموشی رمز عبور
    //    /// </summary>
    //    [HttpPost("forgot-password")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> ForgotPassword([FromBody] string email)
    //    {
    //        try
    //        {
    //            var result = await _authService.ForgotPasswordAsync(email);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// بازنشانی رمز عبور
    //    /// </summary>
    //    [HttpPost("reset-password")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    //    {
    //        try
    //        {
    //            var result = await _authService.ResetPasswordAsync(resetPasswordDto);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// تایید ایمیل
    //    /// </summary>
    //    [HttpPost("confirm-email")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    //    {
    //        try
    //        {
    //            var result = await _authService.ConfirmEmailAsync(userId, token);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// ارسال مجدد تایید ایمیل
    //    /// </summary>
    //    [HttpPost("resend-email-confirmation")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> ResendEmailConfirmation([FromBody] string email)
    //    {
    //        try
    //        {
    //            var result = await _authService.ResendEmailConfirmationAsync(email);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    #endregion

    //    #region Two-Factor Authentication

    //    /// <summary>
    //    /// فعال‌سازی احراز هویت دو مرحله‌ای
    //    /// </summary>
    //    [HttpPost("enable-2fa")]
    //    [Authorize]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> EnableTwoFactor()
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<bool>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _authService.EnableTwoFactorAsync(userId);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// غیرفعال‌سازی احراز هویت دو مرحله‌ای
    //    /// </summary>
    //    [HttpPost("disable-2fa")]
    //    [Authorize]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> DisableTwoFactor()
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<bool>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _authService.DisableTwoFactorAsync(userId);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// بررسی وضعیت احراز هویت دو مرحله‌ای
    //    /// </summary>
    //    [HttpGet("2fa-status")]
    //    [Authorize]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> GetTwoFactorStatus()
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<bool>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _authService.IsTwoFactorEnabledAsync(userId);
    //            return Ok(ApiResponseDto<bool>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<bool>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// تولید کلید احراز هویت دو مرحله‌ای
    //    /// </summary>
    //    [HttpPost("generate-2fa-secret")]
    //    [Authorize]
    //    public async Task<ActionResult<ApiResponseDto<string>>> GenerateTwoFactorSecret()
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<string>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _authService.GenerateTwoFactorSecretAsync(userId);
    //            return Ok(ApiResponseDto<string>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<string>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// تایید کد احراز هویت دو مرحله‌ای
    //    /// </summary>
    //    [HttpPost("verify-2fa-code")]
    //    public async Task<ActionResult<ApiResponseDto<bool>>> VerifyTwoFactorCode([FromBody] string userId, [FromBody] string code)
    //    {
    //        try
    //        {
    //            var result = await _authService.VerifyTwoFactorCodeAsync(userId, code);
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
    //    /// دریافت آمار احراز هویت
    //    /// </summary>
    //    [HttpGet("analytics")]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<ActionResult<ApiResponseDto<AuthAnalyticsDto>>> GetAuthAnalytics()
    //    {
    //        try
    //        {
    //            var result = await _authService.GetAuthAnalyticsAsync();
    //            return Ok(ApiResponseDto<AuthAnalyticsDto>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<AuthAnalyticsDto>.Error(ex.Message));
    //        }
    //    }

    //    /// <summary>
    //    /// دریافت تاریخچه ورود کاربر
    //    /// </summary>
    //    [HttpGet("login-history")]
    //    [Authorize]
    //    public async Task<ActionResult<ApiResponseDto<IEnumerable<LoginHistoryDto>>>> GetLoginHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    //    {
    //        try
    //        {
    //            var userId = User.FindFirst("sub")?.Value;
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                return Unauthorized(ApiResponseDto<IEnumerable<LoginHistoryDto>>.Error("کاربر احراز هویت نشده است"));
    //            }

    //            var result = await _authService.GetLoginHistoryAsync(userId, page, pageSize);
    //            return Ok(ApiResponseDto<IEnumerable<LoginHistoryDto>>.Success(result));
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ApiResponseDto<IEnumerable<LoginHistoryDto>>.Error(ex.Message));
    //        }
    //    }

    //    #endregion
    //}
} 