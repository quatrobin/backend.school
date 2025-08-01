using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApplication3.Models.Common;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Teacher)]
[SwaggerTag("API для преподавателей")]
public class AdminController : ControllerBase
{
    [HttpGet("dashboard")]
    [SwaggerOperation(
        Summary = "Панель управления преподавателя",
        Description = "Демонстрирует доступ только для пользователей с ролью 'Преподаватель'"
    )]
    [SwaggerResponse(200, "Доступ разрешен", typeof(BaseResponse<object>))]
    [SwaggerResponse(401, "Неверный токен", typeof(BaseResponse<object>))]
    [SwaggerResponse(403, "Доступ запрещен", typeof(BaseResponse<object>))]
    public BaseResponse<object> GetDashboard()
    {
        var data = new 
        { 
            message = "Добро пожаловать в панель управления преподавателя",
            features = new[] { "Управление курсами", "Просмотр учеников", "Создание заданий" }
        };
        return BaseResponse<object>.SuccessResponse(data, "Доступ разрешен");
    }
    
    [HttpGet("students")]
    [SwaggerOperation(
        Summary = "Список учеников",
        Description = "Возвращает список всех учеников (только для преподавателей)"
    )]
    [SwaggerResponse(200, "Список получен успешно", typeof(BaseResponse<object>))]
    public BaseResponse<object> GetStudents()
    {
        var students = new[]
        {
            new { id = 1, name = "Иван Иванов", email = "ivan@example.com" },
            new { id = 2, name = "Мария Петрова", email = "maria@example.com" }
        };
        
        return BaseResponse<object>.SuccessResponse(students, "Список учеников получен");
    }
} 