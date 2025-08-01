using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Requests;

public class UpdateBookRequest
{
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
    
    public List<int> CourseIds { get; set; } = new List<int>();
} 