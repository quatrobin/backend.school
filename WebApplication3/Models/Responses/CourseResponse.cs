namespace WebApplication3.Models.Responses;

public class CourseResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int StudentsCount { get; set; }
    public int LessonsCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 