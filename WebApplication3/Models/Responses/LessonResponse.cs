namespace WebApplication3.Models.Responses;

public class LessonResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public DateTime LessonDate { get; set; }
    public int DurationMinutes { get; set; }
    public string? Materials { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 