using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Models.Entities;

namespace WebApplication3.Services.Interfaces;

public interface IAuthService
{
    Task<BaseResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<BaseResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequest request);
    Task<BaseResponse<List<RoleResponse>>> GetRolesAsync();
    Task<User?> GetUserByIdAsync(int userId);
} 