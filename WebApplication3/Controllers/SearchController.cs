using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Serilog;
using WebApplication3.Models.Common;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SearchController : BaseController
{
    private readonly IElasticsearchService _elasticsearchService;

    public SearchController(IElasticsearchService elasticsearchService)
    {
        _elasticsearchService = elasticsearchService;
    }

    /// <summary>
    /// Глобальный поиск по всем индексам Elasticsearch
    /// </summary>
    /// <param name="query">Поисковый запрос</param>
    /// <returns>Результаты поиска по всем типам данных</returns>
    [HttpGet("global")]
    [SwaggerOperation(
        Summary = "Глобальный поиск",
        Description = "Выполняет поиск по всем индексам: пользователи, курсы, книги, уроки, задания",
        OperationId = "SearchGlobal",
        Tags = new[] { "Поиск" }
    )]
    [SwaggerResponse(200, "Успешный поиск", typeof(object))]
    [SwaggerResponse(400, "Неверный запрос")]
    [SwaggerResponse(500, "Ошибка сервера")]
    public async Task<IActionResult> GlobalSearch([FromQuery] string query)
    {
        Log.Information("Запрос глобального поиска. Запрос: {Query}", query);
        
        if (string.IsNullOrWhiteSpace(query))
        {
            Log.Warning("Пустой поисковый запрос");
            return BadRequest("Query parameter is required");
        }

        try
        {
            var results = new Dictionary<string, object>();

            // Поиск по пользователям
            var users = await _elasticsearchService.SearchAsync<object>(query, "users");
            if (users.Any())
                results["users"] = users;

            // Поиск по курсам
            var courses = await _elasticsearchService.SearchAsync<object>(query, "courses");
            if (courses.Any())
                results["courses"] = courses;

            // Поиск по книгам
            var books = await _elasticsearchService.SearchAsync<object>(query, "books");
            if (books.Any())
                results["books"] = books;

            // Поиск по урокам
            var lessons = await _elasticsearchService.SearchAsync<object>(query, "lessons");
            if (lessons.Any())
                results["lessons"] = lessons;

            // Поиск по заданиям
            var assignments = await _elasticsearchService.SearchAsync<object>(query, "assignments");
            if (assignments.Any())
                results["assignments"] = assignments;

            var totalResults = results.Values.Sum(v => ((List<object>)v).Count);
            Log.Information("Глобальный поиск завершен. Запрос: {Query}, Найдено результатов: {TotalResults}", 
                query, totalResults);

            return Ok(new
            {
                query = query,
                total_results = totalResults,
                results = results
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при выполнении глобального поиска. Запрос: {Query}", query);
            return StatusCode(500, new { error = "Search failed", message = ex.Message });
        }
    }

    /// <summary>
    /// Поиск по конкретному индексу Elasticsearch
    /// </summary>
    /// <param name="indexName">Название индекса (users, courses, books, lessons, assignments)</param>
    /// <param name="query">Поисковый запрос</param>
    /// <returns>Результаты поиска по указанному индексу</returns>
    [HttpGet("{indexName}")]
    [SwaggerOperation(
        Summary = "Поиск по индексу",
        Description = "Выполняет поиск по конкретному индексу Elasticsearch",
        OperationId = "SearchByIndex",
        Tags = new[] { "Поиск" }
    )]
    [SwaggerResponse(200, "Успешный поиск", typeof(object))]
    [SwaggerResponse(400, "Неверный запрос")]
    [SwaggerResponse(500, "Ошибка сервера")]
    public async Task<IActionResult> SearchByIndex(string indexName, [FromQuery] string query)
    {
        Log.Information("Запрос поиска по индексу. Индекс: {IndexName}, Запрос: {Query}", indexName, query);
        
        if (string.IsNullOrWhiteSpace(query))
        {
            Log.Warning("Пустой поисковый запрос для индекса {IndexName}", indexName);
            return BadRequest("Query parameter is required");
        }

        if (string.IsNullOrWhiteSpace(indexName))
        {
            Log.Warning("Пустое название индекса");
            return BadRequest("Index name is required");
        }

        try
        {
            var results = await _elasticsearchService.SearchAsync<object>(query, indexName);
            
            Log.Information("Поиск по индексу завершен. Индекс: {IndexName}, Запрос: {Query}, Найдено: {Count}", 
                indexName, query, results.Count);
            
            return Ok(new
            {
                index = indexName,
                query = query,
                total_results = results.Count,
                results = results
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при поиске по индексу. Индекс: {IndexName}, Запрос: {Query}", indexName, query);
            return StatusCode(500, new { error = "Search failed", message = ex.Message });
        }
    }

    /// <summary>
    /// Проверка статуса подключения к Elasticsearch
    /// </summary>
    /// <returns>Статус подключения и состояние индексов</returns>
    [HttpGet("status")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Статус Elasticsearch",
        Description = "Проверяет подключение к Elasticsearch и состояние всех индексов",
        OperationId = "GetElasticsearchStatus",
        Tags = new[] { "Система" }
    )]
    [SwaggerResponse(200, "Статус получен", typeof(object))]
    [SwaggerResponse(500, "Ошибка подключения")]
    public async Task<IActionResult> GetStatus()
    {
        Log.Information("Запрос статуса Elasticsearch");
        
        try
        {
            var indices = new[] { "users", "courses", "books", "lessons", "assignments" };
            var status = new Dictionary<string, bool>();

            foreach (var index in indices)
            {
                status[index] = await _elasticsearchService.IndexExistsAsync(index);
            }

            Log.Information("Статус Elasticsearch получен. Индексы: {@Indices}", status);
            
            return Ok(new
            {
                status = "connected",
                indices = status
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при получении статуса Elasticsearch");
            return StatusCode(500, new { status = "disconnected", error = ex.Message });
        }
    }

    /// <summary>
    /// Создание всех необходимых индексов в Elasticsearch
    /// </summary>
    /// <returns>Результат создания индексов</returns>
    [HttpPost("setup")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Создание индексов",
        Description = "Создает все необходимые индексы в Elasticsearch: users, courses, books, lessons, assignments",
        OperationId = "SetupElasticsearchIndices",
        Tags = new[] { "Администрирование" }
    )]
    [SwaggerResponse(200, "Индексы созданы", typeof(object))]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(403, "Недостаточно прав")]
    [SwaggerResponse(500, "Ошибка создания индексов")]
    public async Task<IActionResult> SetupIndices()
    {
        Log.Information("Запрос на создание индексов Elasticsearch");
        
        try
        {
            var indices = new[] { "users", "courses", "books", "lessons", "assignments" };
            var results = new Dictionary<string, bool>();

            foreach (var index in indices)
            {
                var exists = await _elasticsearchService.IndexExistsAsync(index);
                if (!exists)
                {
                    results[index] = await _elasticsearchService.CreateIndexAsync(index);
                }
                else
                {
                    results[index] = true; // уже существует
                }
            }

            Log.Information("Создание индексов завершено. Результаты: {@Results}", results);
            
            return Ok(new
            {
                message = "Indices setup completed",
                results = results
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при создании индексов Elasticsearch");
            return StatusCode(500, new { error = "Setup failed", message = ex.Message });
        }
    }
} 