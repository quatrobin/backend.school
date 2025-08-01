using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class GradeSubmissionRequest
{
    [Required]
    [Range(0, 100)]
    public int Score { get; set; }
    
    [MaxLength(1000)]
    public string? Feedback { get; set; }
} 