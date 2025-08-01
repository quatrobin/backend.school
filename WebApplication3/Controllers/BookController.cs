using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    /// Получить все книги
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<BaseResponse<List<BookResponse>>> GetAllBooks()
    {
        return await _bookService.GetAllBooksAsync();
    }

    /// <summary>
    /// Получить книгу по ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<BaseResponse<BookResponse>> GetBookById(int id)
    {
        return await _bookService.GetBookByIdAsync(id);
    }

    /// <summary>
    /// Получить книги по курсу
    /// </summary>
    [HttpGet("course/{courseId}")]
    [Authorize]
    public async Task<BaseResponse<List<BookResponse>>> GetBooksByCourse(int courseId)
    {
        return await _bookService.GetBooksByCourseAsync(courseId);
    }

    /// <summary>
    /// Создать новую книгу
    /// </summary>
    [HttpPost]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<BookResponse>> CreateBook([FromBody] CreateBookRequest request)
    {
        return await _bookService.CreateBookAsync(request);
    }

    /// <summary>
    /// Обновить книгу
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<BookResponse>> UpdateBook(int id, [FromBody] UpdateBookRequest request)
    {
        return await _bookService.UpdateBookAsync(id, request);
    }

    /// <summary>
    /// Удалить книгу
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse> DeleteBook(int id)
    {
        return await _bookService.DeleteBookAsync(id);
    }

    /// <summary>
    /// Добавить книгу к курсу
    /// </summary>
    [HttpPost("{bookId}/course/{courseId}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse> AddBookToCourse(int bookId, int courseId)
    {
        return await _bookService.AddBookToCourseAsync(bookId, courseId);
    }

    /// <summary>
    /// Удалить книгу из курса
    /// </summary>
    [HttpDelete("{bookId}/course/{courseId}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse> RemoveBookFromCourse(int bookId, int courseId)
    {
        return await _bookService.RemoveBookFromCourseAsync(bookId, courseId);
    }
} 