using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Entities;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Навигационные свойства
    public ICollection<CourseEnrollment> Enrollments { get; set; } = new List<CourseEnrollment>();
} 