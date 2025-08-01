using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Entities;

public class Book
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(50)]
    public string? ISBN { get; set; }
    
    public int PublicationYear { get; set; }
    
    [MaxLength(100)]
    public string? Publisher { get; set; }
    
    public int Pages { get; set; }
    
    [MaxLength(20)]
    public string? Language { get; set; } = "Русский";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Связь с курсами (многие ко многим)
    public ICollection<Course> Courses { get; set; } = new List<Course>();
} 