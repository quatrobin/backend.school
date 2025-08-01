using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Entities;

public class Lesson
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    public DateTime LessonDate { get; set; }
    
    public int DurationMinutes { get; set; } = 90;
    
    [MaxLength(500)]
    public string? Materials { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
} 