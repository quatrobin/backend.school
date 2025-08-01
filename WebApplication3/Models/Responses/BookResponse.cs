namespace WebApplication3.Models.Responses;

public class BookResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public int PublicationYear { get; set; }
    public string? Publisher { get; set; }
    public int Pages { get; set; }
    public string? Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CourseResponse> Courses { get; set; } = new List<CourseResponse>();
} 