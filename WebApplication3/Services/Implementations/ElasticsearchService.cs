using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using Serilog;
using WebApplication3.Models.Common;
using WebApplication3.Models.Entities;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Services.Implementations;

public class ElasticsearchService : IElasticsearchService
{
    private readonly IElasticClient _elasticClient;
    private readonly ElasticsearchSettings _settings;

    public ElasticsearchService(IOptions<ElasticsearchSettings> settings)
    {
        _settings = settings.Value;
        
        try
        {
            Log.Information("Инициализация Elasticsearch клиента. URL: {ElasticsearchUrl}, Индекс по умолчанию: {DefaultIndex}", 
                _settings.Url, _settings.DefaultIndex);
            
            var pool = new SingleNodeConnectionPool(new Uri(_settings.Url));
            var connectionSettings = new ConnectionSettings(pool)
                .DefaultIndex(_settings.DefaultIndex)
                .EnableApiVersioningHeader()
                .DisableDirectStreaming()
                .OnRequestCompleted(details =>
                {
                    if (details.Success)
                    {
                        Log.Debug("Elasticsearch запрос выполнен успешно. URL: {Url}, Метод: {Method}", 
                            details.Uri, details.HttpMethod);
                    }
                    else
                    {
                        Log.Error("Elasticsearch запрос завершился с ошибкой. URL: {Url}, Метод: {Method}, Ошибка: {Error}", 
                            details.Uri, details.HttpMethod, details.OriginalException?.Message);
                    }
                });

            _elasticClient = new ElasticClient(connectionSettings);
            
            Log.Information("Elasticsearch клиент успешно инициализирован");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка инициализации Elasticsearch клиента. URL: {ElasticsearchUrl}", _settings.Url);
            throw;
        }
    }

    public async Task<bool> IndexDocumentAsync<T>(T document, string indexName) where T : class
    {
        try
        {
            Log.Information("Индексация документа в Elasticsearch. Индекс: {IndexName}, Тип: {DocumentType}", 
                indexName, typeof(T).Name);
            
            var response = await _elasticClient.IndexAsync(document, i => i.Index(indexName));
            
            if (response.IsValid)
            {
                Log.Information("Документ успешно проиндексирован. ID: {DocumentId}, Индекс: {IndexName}", 
                    response.Id, indexName);
                return true;
            }
            else
            {
                Log.Warning("Ошибка индексации документа. Индекс: {IndexName}, Ошибка: {Error}", 
                    indexName, response.ServerError?.Error?.Reason);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при индексации документа. Индекс: {IndexName}, Тип: {DocumentType}", 
                indexName, typeof(T).Name);
            return false;
        }
    }

    public async Task<bool> IndexUserAsync(User user)
    {
        Log.Information("Индексация пользователя. ID: {UserId}, Email: {Email}", user.Id, user.Email);
        
        var userDocument = new
        {
            id = user.Id,
            first_name = user.FirstName,
            last_name = user.LastName,
            email = user.Email,
            role = user.Role?.Name,
            type = "user",
            created_at = user.CreatedAt
        };

        return await IndexDocumentAsync(userDocument, "users");
    }

    public async Task<bool> IndexCourseAsync(Course course)
    {
        Log.Information("Индексация курса. ID: {CourseId}, Название: {CourseName}", course.Id, course.Name);
        
        var courseDocument = new
        {
            id = course.Id,
            name = course.Name,
            description = course.Description,
            type = "course",
            created_at = course.CreatedAt
        };

        return await IndexDocumentAsync(courseDocument, "courses");
    }

    public async Task<bool> IndexBookAsync(Book book)
    {
        Log.Information("Индексация книги. ID: {BookId}, Название: {BookTitle}, Автор: {BookAuthor}", 
            book.Id, book.Title, book.Author);
        
        var bookDocument = new
        {
            id = book.Id,
            title = book.Title,
            author = book.Author,
            isbn = book.ISBN,
            type = "book",
            created_at = book.CreatedAt
        };

        return await IndexDocumentAsync(bookDocument, "books");
    }

    public async Task<bool> IndexLessonAsync(Lesson lesson)
    {
        Log.Information("Индексация урока. ID: {LessonId}, Название: {LessonTitle}, Курс: {CourseId}", 
            lesson.Id, lesson.Title, lesson.CourseId);
        
        var lessonDocument = new
        {
            id = lesson.Id,
            title = lesson.Title,
            description = lesson.Description,
            materials = lesson.Materials,
            course_id = lesson.CourseId,
            type = "lesson",
            created_at = lesson.CreatedAt
        };

        return await IndexDocumentAsync(lessonDocument, "lessons");
    }

    public async Task<bool> IndexAssignmentAsync(Assignment assignment)
    {
        Log.Information("Индексация задания. ID: {AssignmentId}, Название: {AssignmentTitle}, Курс: {CourseId}", 
            assignment.Id, assignment.Title, assignment.CourseId);
        
        var assignmentDocument = new
        {
            id = assignment.Id,
            title = assignment.Title,
            description = assignment.Description,
            course_id = assignment.CourseId,
            type = "assignment",
            created_at = assignment.CreatedAt
        };

        return await IndexDocumentAsync(assignmentDocument, "assignments");
    }

    public async Task<List<T>> SearchAsync<T>(string searchTerm, string indexName) where T : class
    {
        try
        {
            Log.Information("Поиск в Elasticsearch. Запрос: {SearchTerm}, Индекс: {IndexName}, Тип: {DocumentType}", 
                searchTerm, indexName, typeof(T).Name);
            
            var searchRequest = new SearchRequest(indexName)
            {
                Query = new MultiMatchQuery
                {
                    Query = searchTerm,
                    Fields = new[] { "*" },
                    Fuzziness = Fuzziness.Auto
                }
            };

            var response = await _elasticClient.SearchAsync<T>(searchRequest);
            
            if (response.IsValid)
            {
                Log.Information("Поиск завершен успешно. Найдено документов: {Count}, Индекс: {IndexName}", 
                    response.Documents.Count, indexName);
                return response.Documents.ToList();
            }
            else
            {
                Log.Warning("Ошибка поиска в Elasticsearch. Индекс: {IndexName}, Ошибка: {Error}", 
                    indexName, response.ServerError?.Error?.Reason);
                return new List<T>();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при поиске в Elasticsearch. Запрос: {SearchTerm}, Индекс: {IndexName}", 
                searchTerm, indexName);
            return new List<T>();
        }
    }

    public async Task<bool> DeleteDocumentAsync<T>(string id, string indexName) where T : class
    {
        try
        {
            Log.Information("Удаление документа из Elasticsearch. ID: {DocumentId}, Индекс: {IndexName}, Тип: {DocumentType}", 
                id, indexName, typeof(T).Name);
            
            var response = await _elasticClient.DeleteAsync<T>(id, d => d.Index(indexName));
            
            if (response.IsValid)
            {
                Log.Information("Документ успешно удален из Elasticsearch. ID: {DocumentId}, Индекс: {IndexName}", 
                    id, indexName);
                return true;
            }
            else
            {
                Log.Warning("Ошибка удаления документа из Elasticsearch. ID: {DocumentId}, Индекс: {IndexName}, Ошибка: {Error}", 
                    id, indexName, response.ServerError?.Error?.Reason);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при удалении документа из Elasticsearch. ID: {DocumentId}, Индекс: {IndexName}", 
                id, indexName);
            return false;
        }
    }

    public async Task<bool> UpdateDocumentAsync<T>(T document, string id, string indexName) where T : class
    {
        try
        {
            Log.Information("Обновление документа в Elasticsearch. ID: {DocumentId}, Индекс: {IndexName}, Тип: {DocumentType}", 
                id, indexName, typeof(T).Name);
            
            var response = await _elasticClient.UpdateAsync<T>(id, u => u.Index(indexName).Doc(document));
            
            if (response.IsValid)
            {
                Log.Information("Документ успешно обновлен в Elasticsearch. ID: {DocumentId}, Индекс: {IndexName}", 
                    id, indexName);
                return true;
            }
            else
            {
                Log.Warning("Ошибка обновления документа в Elasticsearch. ID: {DocumentId}, Индекс: {IndexName}, Ошибка: {Error}", 
                    id, indexName, response.ServerError?.Error?.Reason);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при обновлении документа в Elasticsearch. ID: {DocumentId}, Индекс: {IndexName}", 
                id, indexName);
            return false;
        }
    }

    public async Task<bool> CreateIndexAsync(string indexName)
    {
        try
        {
            Log.Information("Создание индекса в Elasticsearch. Индекс: {IndexName}", indexName);
            
            var response = await _elasticClient.Indices.CreateAsync(indexName);
            
            if (response.IsValid)
            {
                Log.Information("Индекс успешно создан в Elasticsearch. Индекс: {IndexName}", indexName);
                return true;
            }
            else
            {
                Log.Warning("Ошибка создания индекса в Elasticsearch. Индекс: {IndexName}, Ошибка: {Error}", 
                    indexName, response.ServerError?.Error?.Reason);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при создании индекса в Elasticsearch. Индекс: {IndexName}", indexName);
            return false;
        }
    }

    public async Task<bool> IndexExistsAsync(string indexName)
    {
        try
        {
            var response = await _elasticClient.Indices.ExistsAsync(indexName);
            
            if (response.Exists)
            {
                Log.Debug("Индекс существует в Elasticsearch. Индекс: {IndexName}", indexName);
            }
            else
            {
                Log.Debug("Индекс не существует в Elasticsearch. Индекс: {IndexName}", indexName);
            }
            
            return response.Exists;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при проверке существования индекса в Elasticsearch. Индекс: {IndexName}", indexName);
            return false;
        }
    }

    public async Task<bool> IsElasticsearchAvailableAsync()
    {
        try
        {
            Log.Information("Проверка доступности Elasticsearch. URL: {ElasticsearchUrl}", _settings.Url);
            
            var response = await _elasticClient.PingAsync();
            
            if (response.IsValid)
            {
                Log.Information("Elasticsearch доступен");
                return true;
            }
            else
            {
                Log.Error("Elasticsearch недоступен. Ошибка: {Error}", 
                    response.ServerError?.Error?.Reason);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при проверке доступности Elasticsearch. URL: {ElasticsearchUrl}", _settings.Url);
            return false;
        }
    }

    public async Task<Dictionary<string, object>> GetElasticsearchInfoAsync()
    {
        try
        {
            Log.Information("Получение информации о Elasticsearch");
            
            var info = await _elasticClient.RootNodeInfoAsync();
            
            if (info.IsValid)
            {
                var infoDict = new Dictionary<string, object>
                {
                    ["version"] = info.Version?.Number ?? "unknown",
                    ["cluster_name"] = info.ClusterName ?? "unknown",
                    ["tagline"] = info.Tagline ?? "unknown",
                    ["status"] = "available"
                };
                
                Log.Information("Информация о Elasticsearch получена. Версия: {Version}, Кластер: {ClusterName}", 
                    info.Version?.Number, info.ClusterName);
                
                return infoDict;
            }
            else
            {
                Log.Error("Ошибка получения информации о Elasticsearch. Ошибка: {Error}", 
                    info.ServerError?.Error?.Reason);
                return new Dictionary<string, object> { ["status"] = "error" };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при получении информации о Elasticsearch");
            return new Dictionary<string, object> { ["status"] = "error" };
        }
    }
} 