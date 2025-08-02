using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
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

    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    /// <param name="request">Данные для входа</param>
    /// <returns>JWT токен и информация о пользователе</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Вход в систему",
        Description = "Аутентифицирует пользователя и возвращает JWT токен",
        OperationId = "Login",
        Tags = new[] { "Аутентификация" }
    )]
    [SwaggerResponse(200, "Успешная аутентификация", typeof(BaseResponse<AuthResponse>))]
    [SwaggerResponse(400, "Неверные данные")]
    [SwaggerResponse(401, "Неверные учетные данные")]
    public async Task<BaseResponse<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            Log.Information("Попытка входа пользователя. Email: {Email}", request.Email);
            var result = await _authService.LoginAsync(request);
            
            if (result.Success)
            {
                Log.Information("Успешный вход пользователя. Email: {Email}, UserId: {UserId}", 
                    request.Email, result.Data?.User?.Id);
            }
            else
            {
                Log.Warning("Неудачная попытка входа. Email: {Email}, Ошибка: {Error}", 
                    request.Email, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при входе пользователя. Email: {Email}", request.Email);
            return BaseResponse<AuthResponse>.ErrorResponse($"Ошибка входа: {ex.Message}");
        }
    }

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <param name="request">Данные для регистрации</param>
    /// <returns>JWT токен и информация о пользователе</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Регистрация",
        Description = "Регистрирует нового пользователя и возвращает JWT токен",
        OperationId = "Register",
        Tags = new[] { "Аутентификация" }
    )]
    [SwaggerResponse(200, "Успешная регистрация", typeof(BaseResponse<AuthResponse>))]
    [SwaggerResponse(400, "Неверные данные")]
    [SwaggerResponse(409, "Пользователь уже существует")]
    public async Task<BaseResponse<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            Log.Information("Попытка регистрации пользователя. Email: {Email}, Имя: {FirstName} {LastName}", 
                request.Email, request.FirstName, request.LastName);
            var result = await _authService.RegisterAsync(request);
            
            if (result.Success)
            {
                Log.Information("Успешная регистрация пользователя. Email: {Email}, UserId: {UserId}", 
                    request.Email, result.Data?.User?.Id);
            }
            else
            {
                Log.Warning("Неудачная попытка регистрации. Email: {Email}, Ошибка: {Error}", 
                    request.Email, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при регистрации пользователя. Email: {Email}", request.Email);
            return BaseResponse<AuthResponse>.ErrorResponse($"Ошибка регистрации: {ex.Message}");
        }
    }

    /// <summary>
    /// Получение профиля текущего пользователя
    /// </summary>
    /// <returns>Информация о профиле пользователя</returns>
    [HttpGet("profile")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Получить профиль",
        Description = "Возвращает профиль текущего авторизованного пользователя",
        OperationId = "GetProfile",
        Tags = new[] { "Аутентификация" }
    )]
    [SwaggerResponse(200, "Профиль получен", typeof(BaseResponse<UserProfileResponse>))]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(404, "Пользователь не найден")]
    public async Task<BaseResponse<UserProfileResponse>> GetProfile()
    {
        try
        {
            var userId = GetCurrentUserId();
            Log.Information("Запрос профиля пользователя. UserId: {UserId}", userId);
            
            var user = await _authService.GetUserByIdAsync(userId);
            
            if (user == null)
            {
                Log.Warning("Пользователь не найден при запросе профиля. UserId: {UserId}", userId);
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

            Log.Information("Профиль пользователя загружен успешно. UserId: {UserId}, Email: {Email}", 
                userId, user.Email);
            return SuccessResponse(profile, "Профиль успешно загружен");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при загрузке профиля пользователя. UserId: {UserId}", 
                GetCurrentUserId());
            return ErrorResponse<UserProfileResponse>($"Ошибка при загрузке профиля: {ex.Message}");
        }
    }

    /// <summary>
    /// Изменение пароля пользователя
    /// </summary>
    /// <param name="request">Данные для изменения пароля</param>
    /// <returns>Результат изменения пароля</returns>
    [HttpPost("change-password")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Изменить пароль",
        Description = "Изменяет пароль текущего авторизованного пользователя",
        OperationId = "ChangePassword",
        Tags = new[] { "Аутентификация" }
    )]
    [SwaggerResponse(200, "Пароль изменен", typeof(BaseResponse))]
    [SwaggerResponse(400, "Неверные данные")]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(400, "Неверный текущий пароль")]
    public async Task<BaseResponse> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            Log.Information("Попытка смены пароля пользователя. UserId: {UserId}", userId);
            
            var result = await _authService.ChangePasswordAsync(request);
            
            if (result.Success)
            {
                Log.Information("Пароль пользователя изменен успешно. UserId: {UserId}", userId);
            }
            else
            {
                Log.Warning("Ошибка смены пароля. UserId: {UserId}, Ошибка: {Error}", 
                    userId, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            var userId = GetCurrentUserId();
            Log.Error(ex, "Исключение при смене пароля пользователя. UserId: {UserId}", userId);
            return BaseResponse.ErrorResponse($"Ошибка смены пароля: {ex.Message}");
        }
    }
} 