using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<BaseResponse<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        return await _authService.LoginAsync(request);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<BaseResponse<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        return await _authService.RegisterAsync(request);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<BaseResponse<UserProfileResponse>> GetProfile()
    {
        try
        {
            var userId = GetCurrentUserId();
            var user = await _authService.GetUserByIdAsync(userId);
            
            if (user == null)
            {
                return ErrorResponse<UserProfileResponse>("Пользователь не найден");
            }

            var profile = new UserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.Name
            };

            return SuccessResponse(profile, "Профиль успешно загружен");
        }
        catch (Exception ex)
        {
            return ErrorResponse<UserProfileResponse>($"Ошибка при загрузке профиля: {ex.Message}");
        }
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<BaseResponse> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        return await _authService.ChangePasswordAsync(request);
    }
} 