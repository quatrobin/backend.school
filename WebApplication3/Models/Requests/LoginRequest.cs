using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class LoginRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
} 