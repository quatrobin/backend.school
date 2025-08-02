using WebApplication3.Models.Entities;

namespace WebApplication3.Services.Interfaces;

public interface IElasticsearchService
{
    Task<bool> IndexDocumentAsync<T>(T document, string indexName) where T : class;
    Task<bool> IndexUserAsync(User user);
    Task<bool> IndexCourseAsync(Course course);
    Task<bool> IndexBookAsync(Book book);
    Task<bool> IndexLessonAsync(Lesson lesson);
    Task<bool> IndexAssignmentAsync(Assignment assignment);
    Task<List<T>> SearchAsync<T>(string searchTerm, string indexName) where T : class;
    Task<bool> DeleteDocumentAsync<T>(string id, string indexName) where T : class;
    Task<bool> UpdateDocumentAsync<T>(T document, string id, string indexName) where T : class;
    Task<bool> CreateIndexAsync(string indexName);
    Task<bool> IndexExistsAsync(string indexName);
    Task<bool> IsElasticsearchAvailableAsync();
    Task<Dictionary<string, object>> GetElasticsearchInfoAsync();
} 