using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Entities;

public class Assignment
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    public DateTime DueDate { get; set; }
    
    public int MaxScore { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
} 