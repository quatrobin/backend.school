using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Entities;

public class CourseEnrollment
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    public User Student { get; set; } = null!;
    
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    // Навигационные свойства
    public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
} 