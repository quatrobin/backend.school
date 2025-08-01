using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class CreateCourseRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
} 