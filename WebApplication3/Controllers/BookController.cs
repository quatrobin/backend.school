using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Serilog;
using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : BaseController
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Получить все книги в системе
    /// </summary>
    /// <returns>Список всех книг</returns>
    [HttpGet]
    [Authorize]
    [SwaggerOperation(
        Summary = "Получить все книги",
        Description = "Возвращает список всех книг в системе с информацией о связанных курсах",
        OperationId = "GetAllBooks",
        Tags = new[] { "Книги" }
    )]
    [SwaggerResponse(200, "Книги получены", typeof(BaseResponse<List<BookResponse>>))]
    [SwaggerResponse(401, "Не авторизован")]
    public async Task<BaseResponse<List<BookResponse>>> GetAllBooks()
    {
        try
        {
            Log.Information("Запрос на получение всех книг");
            var result = await _bookService.GetAllBooksAsync();
            
            if (result.Success)
            {
                Log.Information("Книги получены успешно. Количество: {Count}", result.Data?.Count ?? 0);
            }
            else
            {
                Log.Warning("Ошибка получения книг: {Error}", result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при получении всех книг");
            return BaseResponse<List<BookResponse>>.ErrorResponse($"Ошибка получения книг: {ex.Message}");
        }
    }

    /// <summary>
    /// Получить книгу по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор книги</param>
    /// <returns>Информация о книге</returns>
    [HttpGet("{id}")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Получить книгу по ID",
        Description = "Возвращает подробную информацию о книге по её идентификатору",
        OperationId = "GetBookById",
        Tags = new[] { "Книги" }
    )]
    [SwaggerResponse(200, "Книга найдена", typeof(BaseResponse<BookResponse>))]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(404, "Книга не найдена")]
    public async Task<BaseResponse<BookResponse>> GetBookById(int id)
    {
        try
        {
            Log.Information("Запрос на получение книги по ID: {BookId}", id);
            var result = await _bookService.GetBookByIdAsync(id);
            
            if (result.Success)
            {
                Log.Information("Книга найдена. ID: {BookId}, Название: {Title}", id, result.Data?.Title);
            }
            else
            {
                Log.Warning("Книга не найдена. ID: {BookId}, Ошибка: {Error}", id, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при получении книги по ID: {BookId}", id);
            return BaseResponse<BookResponse>.ErrorResponse($"Ошибка получения книги: {ex.Message}");
        }
    }

    /// <summary>
    /// Получить книги по курсу
    /// </summary>
    /// <param name="courseId">Идентификатор курса</param>
    /// <returns>Список книг для указанного курса</returns>
    [HttpGet("course/{courseId}")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Получить книги курса",
        Description = "Возвращает список всех книг, связанных с указанным курсом",
        OperationId = "GetBooksByCourse",
        Tags = new[] { "Книги" }
    )]
    [SwaggerResponse(200, "Книги курса получены", typeof(BaseResponse<List<BookResponse>>))]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(404, "Курс не найден")]
    public async Task<BaseResponse<List<BookResponse>>> GetBooksByCourse(int courseId)
    {
        try
        {
            Log.Information("Запрос на получение книг курса. CourseId: {CourseId}", courseId);
            var result = await _bookService.GetBooksByCourseAsync(courseId);
            
            if (result.Success)
            {
                Log.Information("Книги курса получены. CourseId: {CourseId}, Количество: {Count}", 
                    courseId, result.Data?.Count ?? 0);
            }
            else
            {
                Log.Warning("Ошибка получения книг курса. CourseId: {CourseId}, Ошибка: {Error}", 
                    courseId, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при получении книг курса. CourseId: {CourseId}", courseId);
            return BaseResponse<List<BookResponse>>.ErrorResponse($"Ошибка получения книг курса: {ex.Message}");
        }
    }

    /// <summary>
    /// Создать новую книгу
    /// </summary>
    /// <param name="request">Данные для создания книги</param>
    /// <returns>Созданная книга</returns>
    [HttpPost]
    [Authorize(Roles = Roles.Teacher)]
    [SwaggerOperation(
        Summary = "Создать книгу",
        Description = "Создает новую книгу в системе. Автоматически индексируется в Elasticsearch",
        OperationId = "CreateBook",
        Tags = new[] { "Книги" }
    )]
    [SwaggerResponse(200, "Книга создана", typeof(BaseResponse<BookResponse>))]
    [SwaggerResponse(400, "Неверные данные")]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(403, "Недостаточно прав")]
    public async Task<BaseResponse<BookResponse>> CreateBook([FromBody] CreateBookRequest request)
    {
        try
        {
            Log.Information("Запрос на создание книги. Название: {Title}, Автор: {Author}", 
                request.Title, request.Author);
            var result = await _bookService.CreateBookAsync(request);
            
            if (result.Success)
            {
                Log.Information("Книга создана успешно. ID: {BookId}, Название: {Title}", 
                    result.Data?.Id, result.Data?.Title);
            }
            else
            {
                Log.Warning("Ошибка создания книги. Название: {Title}, Ошибка: {Error}", 
                    request.Title, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при создании книги. Название: {Title}, Автор: {Author}", 
                request.Title, request.Author);
            return BaseResponse<BookResponse>.ErrorResponse($"Ошибка создания книги: {ex.Message}");
        }
    }

    /// <summary>
    /// Обновить книгу
    /// </summary>
    /// <param name="id">Идентификатор книги</param>
    /// <param name="request">Данные для обновления</param>
    /// <returns>Обновленная книга</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    [SwaggerOperation(
        Summary = "Обновить книгу",
        Description = "Обновляет информацию о книге. Автоматически обновляется в Elasticsearch",
        OperationId = "UpdateBook",
        Tags = new[] { "Книги" }
    )]
    [SwaggerResponse(200, "Книга обновлена", typeof(BaseResponse<BookResponse>))]
    [SwaggerResponse(400, "Неверные данные")]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(403, "Недостаточно прав")]
    [SwaggerResponse(404, "Книга не найдена")]
    public async Task<BaseResponse<BookResponse>> UpdateBook(int id, [FromBody] UpdateBookRequest request)
    {
        try
        {
            Log.Information("Запрос на обновление книги. ID: {BookId}, Название: {Title}", 
                id, request.Title);
            var result = await _bookService.UpdateBookAsync(id, request);
            
            if (result.Success)
            {
                Log.Information("Книга обновлена успешно. ID: {BookId}, Название: {Title}", 
                    id, result.Data?.Title);
            }
            else
            {
                Log.Warning("Ошибка обновления книги. ID: {BookId}, Ошибка: {Error}", 
                    id, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при обновлении книги. ID: {BookId}", id);
            return BaseResponse<BookResponse>.ErrorResponse($"Ошибка обновления книги: {ex.Message}");
        }
    }

    /// <summary>
    /// Удалить книгу
    /// </summary>
    /// <param name="id">Идентификатор книги</param>
    /// <returns>Результат удаления</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    [SwaggerOperation(
        Summary = "Удалить книгу",
        Description = "Удаляет книгу из системы. Автоматически удаляется из Elasticsearch",
        OperationId = "DeleteBook",
        Tags = new[] { "Книги" }
    )]
    [SwaggerResponse(200, "Книга удалена", typeof(BaseResponse))]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(403, "Недостаточно прав")]
    [SwaggerResponse(404, "Книга не найдена")]
    public async Task<BaseResponse> DeleteBook(int id)
    {
        try
        {
            Log.Information("Запрос на удаление книги. ID: {BookId}", id);
            var result = await _bookService.DeleteBookAsync(id);
            
            if (result.Success)
            {
                Log.Information("Книга удалена успешно. ID: {BookId}", id);
            }
            else
            {
                Log.Warning("Ошибка удаления книги. ID: {BookId}, Ошибка: {Error}", 
                    id, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при удалении книги. ID: {BookId}", id);
            return BaseResponse.ErrorResponse($"Ошибка удаления книги: {ex.Message}");
        }
    }

    /// <summary>
    /// Добавить книгу к курсу
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="courseId">Идентификатор курса</param>
    /// <returns>Результат добавления</returns>
    [HttpPost("{bookId}/course/{courseId}")]
    [Authorize(Roles = Roles.Teacher)]
    [SwaggerOperation(
        Summary = "Добавить книгу к курсу",
        Description = "Связывает книгу с курсом для использования в обучении",
        OperationId = "AddBookToCourse",
        Tags = new[] { "Книги" }
    )]
    [SwaggerResponse(200, "Книга добавлена к курсу", typeof(BaseResponse))]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(403, "Недостаточно прав")]
    [SwaggerResponse(404, "Книга или курс не найдены")]
    public async Task<BaseResponse> AddBookToCourse(int bookId, int courseId)
    {
        try
        {
            Log.Information("Запрос на добавление книги к курсу. BookId: {BookId}, CourseId: {CourseId}", 
                bookId, courseId);
            var result = await _bookService.AddBookToCourseAsync(bookId, courseId);
            
            if (result.Success)
            {
                Log.Information("Книга добавлена к курсу успешно. BookId: {BookId}, CourseId: {CourseId}", 
                    bookId, courseId);
            }
            else
            {
                Log.Warning("Ошибка добавления книги к курсу. BookId: {BookId}, CourseId: {CourseId}, Ошибка: {Error}", 
                    bookId, courseId, result.Message);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Исключение при добавлении книги к курсу. BookId: {BookId}, CourseId: {CourseId}", 
                bookId, courseId);
            return BaseResponse.ErrorResponse($"Ошибка добавления книги к курсу: {ex.Message}");
        }
    }

    /// <summary>
    /// Удалить книгу из курса
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <param name="courseId">Идентификатор курса</param>
    /// <returns>Результат удаления</returns>
    [HttpDelete("{bookId}/course/{courseId}")]
    [Authorize(Roles = Roles.Teacher)]
    [SwaggerOperation(
        Summary = "Удалить книгу из курса",
        Description = "Удаляет связь между книгой и курсом",
        OperationId = "RemoveBookFromCourse",
        Tags = new[] { "Книги" }
    )]
    [SwaggerResponse(200, "Книга удалена из курса", typeof(BaseResponse))]
    [SwaggerResponse(401, "Не авторизован")]
    [SwaggerResponse(403, "Недостаточно прав")]
    [SwaggerResponse(404, "Книга или курс не найдены")]
    public async Task<BaseResponse> RemoveBookFromCourse(int bookId, int courseId)
    {
        return await _bookService.RemoveBookFromCourseAsync(bookId, courseId);
    }
} 