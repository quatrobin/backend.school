using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController
{
    private readonly IAuthService _authService;

    public UserController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<BaseResponse> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            Log.Information("Попытка смены пароля для пользователя {UserId}", GetCurrentUserId());
            var result = await _authService.ChangePasswordAsync(request);
            
            if (!result.Success)
            {
                // ИСПРАВЛЕНО: Логируется правильный ID пользователя при ошибке
                Log.Error("Ошибка смены пароля для пользователя {UserId} - {Error}", 
                    GetCurrentUserId(), result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            // ИСПРАВЛЕНО: Логируется правильный ID пользователя при исключении
            Log.Error(ex, "Исключение при смене пароля для пользователя {UserId}", 
                GetCurrentUserId());
            return new BaseResponse { Success = false, Message = "Внутренняя ошибка сервера" };
        }
    }
} 