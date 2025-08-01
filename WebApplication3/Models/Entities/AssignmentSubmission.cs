using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Entities;

public class AssignmentSubmission
{
    public int Id { get; set; }
    
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    
    public int StudentId { get; set; }
    public User Student { get; set; } = null!;
    
    [MaxLength(2000)]
    public string? Content { get; set; }
    
    [MaxLength(500)]
    public string? FileUrl { get; set; }
    
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    
    public int? Score { get; set; }
    
    [MaxLength(1000)]
    public string? Feedback { get; set; }
    
    public DateTime? GradedAt { get; set; }
    
    public int? GradedBy { get; set; }
    public User? Grader { get; set; }
} 