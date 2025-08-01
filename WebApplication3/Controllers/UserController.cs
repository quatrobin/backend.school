using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        return await _authService.ChangePasswordAsync(request);
    }
} 