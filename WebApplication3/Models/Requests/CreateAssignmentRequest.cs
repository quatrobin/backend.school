using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class CreateAssignmentRequest
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required]
    [Range(1, 100)]
    public int MaxScore { get; set; }
} 