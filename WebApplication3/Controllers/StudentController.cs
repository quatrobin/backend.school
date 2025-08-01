using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApplication3.Models.Common;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Student)]
[SwaggerTag("API для учеников")]
public class StudentController : ControllerBase
{
    [HttpGet("dashboard")]
    [SwaggerOperation(
        Summary = "Панель ученика",
        Description = "Демонстрирует доступ только для пользователей с ролью 'Ученик'"
    )]
    [SwaggerResponse(200, "Доступ разрешен", typeof(BaseResponse<object>))]
    [SwaggerResponse(401, "Неверный токен", typeof(BaseResponse<object>))]
    [SwaggerResponse(403, "Доступ запрещен", typeof(BaseResponse<object>))]
    public BaseResponse<object> GetDashboard()
    {
        var data = new 
        { 
            message = "Добро пожаловать в панель ученика",
            features = new[] { "Просмотр курсов", "Выполнение заданий", "Просмотр оценок" }
        };
        return BaseResponse<object>.SuccessResponse(data, "Доступ разрешен");
    }
    
    [HttpGet("courses")]
    [SwaggerOperation(
        Summary = "Мои курсы",
        Description = "Возвращает список курсов ученика"
    )]
    [SwaggerResponse(200, "Список получен успешно", typeof(BaseResponse<object>))]
    public BaseResponse<object> GetMyCourses()
    {
        var courses = new[]
        {
            new { id = 1, name = "Математика", teacher = "Петров А.А.", progress = 75 },
            new { id = 2, name = "Физика", teacher = "Сидоров И.И.", progress = 45 }
        };
        
        return BaseResponse<object>.SuccessResponse(courses, "Список курсов получен");
    }
    
    [HttpGet("assignments")]
    [SwaggerOperation(
        Summary = "Мои задания",
        Description = "Возвращает список заданий ученика"
    )]
    [SwaggerResponse(200, "Список получен успешно", typeof(BaseResponse<object>))]
    public BaseResponse<object> GetMyAssignments()
    {
        var assignments = new[]
        {
            new { id = 1, course = "Математика", title = "Решение уравнений", dueDate = "2024-01-15", status = "В процессе" },
            new { id = 2, course = "Физика", title = "Лабораторная работа", dueDate = "2024-01-20", status = "Завершено" }
        };
        
        return BaseResponse<object>.SuccessResponse(assignments, "Список заданий получен");
    }
} 