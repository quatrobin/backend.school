using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class CreateSubmissionRequest
{
    [Required]
    public int AssignmentId { get; set; }
    
    [MaxLength(2000)]
    public string? Content { get; set; }
    
    [MaxLength(500)]
    public string? FileUrl { get; set; }
} 