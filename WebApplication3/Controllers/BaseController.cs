using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication3.Models.Common;

namespace WebApplication3.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }
        return 0; // Возвращаем 0 если пользователь не найден
    }

    protected string GetCurrentUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role);
        return roleClaim?.Value ?? string.Empty;
    }

    protected BaseResponse<T> SuccessResponse<T>(T data, string message = "Успешно")
    {
        return new BaseResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    protected BaseResponse SuccessResponse(string message = "Успешно")
    {
        return new BaseResponse
        {
            Success = true,
            Message = message
        };
    }

    protected BaseResponse<T> ErrorResponse<T>(string message)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message
        };
    }

    protected BaseResponse ErrorResponse(string message)
    {
        return new BaseResponse
        {
            Success = false,
            Message = message
        };
    }
} 