using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Entities;

public class Role
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    // Навигационное свойство для связи с пользователями
    public ICollection<User> Users { get; set; } = new List<User>();
} 