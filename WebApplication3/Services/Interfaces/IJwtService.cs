using WebApplication3.Models.Entities;

namespace WebApplication3.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
} 