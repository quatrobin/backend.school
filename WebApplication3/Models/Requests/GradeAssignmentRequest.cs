using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class GradeAssignmentRequest
{
    [Required]
    public int SubmissionId { get; set; }
    
    [Required]
    [Range(0, 100)]
    public int Score { get; set; }
    
    [MaxLength(500)]
    public string? Feedback { get; set; }
} 