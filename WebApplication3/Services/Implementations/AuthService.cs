using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Models.Entities;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<BaseResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return new BaseResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Пользователь не найден"
                };
            }

            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                return new BaseResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Неверный пароль"
                };
            }

            var token = _jwtService.GenerateToken(user);

            var response = new AuthResponse
            {
                Token = token,
                User = new UserProfileResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role.Name
                }
            };

            return new BaseResponse<AuthResponse>
            {
                Success = true,
                Data = response,
                Message = "Вход выполнен успешно"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<AuthResponse>
            {
                Success = false,
                Message = $"Ошибка при входе: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return new BaseResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Пользователь с таким email уже существует"
                };
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == request.Role);

            if (role == null)
            {
                return new BaseResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Указанная роль не найдена"
                };
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            var response = new AuthResponse
            {
                Token = token,
                User = new UserProfileResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = role.Name
                }
            };

            return new BaseResponse<AuthResponse>
            {
                Success = true,
                Data = response,
                Message = "Регистрация выполнена успешно"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<AuthResponse>
            {
                Success = false,
                Message = $"Ошибка при регистрации: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Пользователь не найден"
                };
            }

            if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Неверный текущий пароль"
                };
            }

            user.PasswordHash = HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Пароль успешно изменен"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse
            {
                Success = false,
                Message = $"Ошибка при смене пароля: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<List<RoleResponse>>> GetRolesAsync()
    {
        try
        {
            var roles = await _context.Roles
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            return new BaseResponse<List<RoleResponse>>
            {
                Success = true,
                Data = roles,
                Message = "Роли успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<RoleResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке ролей: {ex.Message}"
            };
        }
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var hashedPassword = HashPassword(password);
        return hashedPassword == hash;
    }
} 