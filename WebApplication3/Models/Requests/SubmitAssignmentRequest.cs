using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class SubmitAssignmentRequest
{
    [Required]
    public int AssignmentId { get; set; }
    
    [MaxLength(2000)]
    public string? Content { get; set; }
} 