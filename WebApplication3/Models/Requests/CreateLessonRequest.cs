using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class CreateLessonRequest
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    
    [Required]
    public DateTime LessonDate { get; set; }
    
    [Range(15, 480)]
    public int DurationMinutes { get; set; } = 90;
    
    [MaxLength(500)]
    public string? Materials { get; set; }
} 