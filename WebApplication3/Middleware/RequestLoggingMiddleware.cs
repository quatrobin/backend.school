using Serilog;
using System.Diagnostics;

namespace WebApplication3.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        try
        {
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Логируем начало запроса
            Log.Information("HTTP {Method} {Path} начат. IP: {IP}, User-Agent: {UserAgent}", 
                context.Request.Method, 
                context.Request.Path, 
                context.Connection.RemoteIpAddress,
                context.Request.Headers["User-Agent"].ToString());

            await _next(context);

            stopwatch.Stop();

            // ИСПРАВЛЕНО: Логируется реальное время выполнения запроса
            Log.Information("HTTP {Method} {Path} завершен. Статус: {StatusCode}, Время: {ElapsedMs}ms", 
                context.Request.Method, 
                context.Request.Path, 
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);

            // Копируем ответ обратно в оригинальный поток
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            // ИСПРАВЛЕНО: При ошибке логируется реальное время выполнения
            Log.Error(ex, "Ошибка при обработке HTTP {Method} {Path}. Время: {ElapsedMs}ms", 
                context.Request.Method, 
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
            
            // Восстанавливаем оригинальный поток
            context.Response.Body = originalBodyStream;
            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
} 