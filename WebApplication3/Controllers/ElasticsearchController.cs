using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApplication3.Models.Common;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ElasticsearchController : BaseController
{
    private readonly IElasticsearchService _elasticsearchService;

    public ElasticsearchController(IElasticsearchService elasticsearchService)
    {
        _elasticsearchService = elasticsearchService;
    }

    /// <summary>
    /// Проверка доступности Elasticsearch
    /// </summary>
    [HttpGet("health")]
    public async Task<IActionResult> CheckHealth()
    {
        try
        {
            Log.Information("Запрос проверки здоровья Elasticsearch от пользователя {UserId}", GetCurrentUserId());
            
            var isAvailable = await _elasticsearchService.IsElasticsearchAvailableAsync();
            
            if (isAvailable)
            {
                Log.Information("Elasticsearch здоров и доступен");
                return Ok(new { status = "healthy", message = "Elasticsearch доступен" });
            }
            else
            {
                Log.Warning("Elasticsearch недоступен");
                return StatusCode(503, new { status = "unhealthy", message = "Elasticsearch недоступен" });
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при проверке здоровья Elasticsearch");
            return StatusCode(500, new { status = "error", message = "Ошибка при проверке здоровья Elasticsearch" });
        }
    }

    /// <summary>
    /// Получение информации о Elasticsearch
    /// </summary>
    [HttpGet("info")]
    public async Task<IActionResult> GetInfo()
    {
        try
        {
            Log.Information("Запрос информации о Elasticsearch от пользователя {UserId}", GetCurrentUserId());
            
            var info = await _elasticsearchService.GetElasticsearchInfoAsync();
            
            return Ok(info);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при получении информации о Elasticsearch");
            return StatusCode(500, new { status = "error", message = "Ошибка при получении информации о Elasticsearch" });
        }
    }

    /// <summary>
    /// Проверка существования индекса
    /// </summary>
    [HttpGet("index/{indexName}/exists")]
    public async Task<IActionResult> CheckIndexExists(string indexName)
    {
        try
        {
            Log.Information("Проверка существования индекса {IndexName} от пользователя {UserId}", indexName, GetCurrentUserId());
            
            var exists = await _elasticsearchService.IndexExistsAsync(indexName);
            
            return Ok(new { indexName, exists });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при проверке существования индекса {IndexName}", indexName);
            return StatusCode(500, new { status = "error", message = "Ошибка при проверке существования индекса" });
        }
    }

    /// <summary>
    /// Создание индекса
    /// </summary>
    [HttpPost("index/{indexName}")]
    public async Task<IActionResult> CreateIndex(string indexName)
    {
        try
        {
            Log.Information("Создание индекса {IndexName} от пользователя {UserId}", indexName, GetCurrentUserId());
            
            var success = await _elasticsearchService.CreateIndexAsync(indexName);
            
            if (success)
            {
                Log.Information("Индекс {IndexName} успешно создан", indexName);
                return Ok(new { indexName, message = "Индекс успешно создан" });
            }
            else
            {
                Log.Warning("Не удалось создать индекс {IndexName}", indexName);
                return BadRequest(new { indexName, message = "Не удалось создать индекс" });
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при создании индекса {IndexName}", indexName);
            return StatusCode(500, new { status = "error", message = "Ошибка при создании индекса" });
        }
    }

    /// <summary>
    /// Тестовое логирование
    /// </summary>
    [HttpPost("test-logging")]
    public IActionResult TestLogging([FromBody] TestLoggingRequest request)
    {
        try
        {
            Log.Information("Тестовое логирование от пользователя {UserId}. Уровень: {Level}, Сообщение: {Message}", 
                GetCurrentUserId(), request.Level, request.Message);

            switch (request.Level.ToLower())
            {
                case "debug":
                    Log.Debug(request.Message);
                    break;
                case "information":
                    Log.Information(request.Message);
                    break;
                case "warning":
                    Log.Warning(request.Message);
                    break;
                case "error":
                    Log.Error(request.Message);
                    break;
                case "fatal":
                    Log.Fatal(request.Message);
                    break;
                default:
                    Log.Information(request.Message);
                    break;
            }

            return Ok(new { message = "Лог записан успешно", level = request.Level, content = request.Message });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при тестовом логировании");
            return StatusCode(500, new { status = "error", message = "Ошибка при тестовом логировании" });
        }
    }
}

public class TestLoggingRequest
{
    public string Level { get; set; } = "Information";
    public string Message { get; set; } = "Тестовое сообщение";
} 