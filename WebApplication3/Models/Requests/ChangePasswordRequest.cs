using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class ChangePasswordRequest
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
} 