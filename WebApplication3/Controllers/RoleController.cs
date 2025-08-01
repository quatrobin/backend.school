using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Models.Responses;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : BaseController
{
    private readonly ApplicationDbContext _context;

    public RoleController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<BaseResponse<List<RoleResponse>>> GetAllRoles()
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
} 