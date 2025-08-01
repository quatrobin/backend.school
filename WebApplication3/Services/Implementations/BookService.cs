using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Models.Entities;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Services.Implementations;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<BookResponse>>> GetAllBooksAsync()
    {
        try
        {
            var books = await _context.Books
                .Include(b => b.Courses)
                .OrderBy(b => b.Title)
                .ToListAsync();

            var bookResponses = books.Select(MapToBookResponse).ToList();
            return BaseResponse<List<BookResponse>>.SuccessResponse(bookResponses, "Книги успешно загружены");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<BookResponse>>.ErrorResponse($"Ошибка при загрузке книг: {ex.Message}");
        }
    }

    public async Task<BaseResponse<BookResponse>> GetBookByIdAsync(int id)
    {
        try
        {
            var book = await _context.Books
                .Include(b => b.Courses)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return BaseResponse<BookResponse>.ErrorResponse("Книга не найдена");

            var bookResponse = MapToBookResponse(book);
            return BaseResponse<BookResponse>.SuccessResponse(bookResponse, "Книга успешно загружена");
        }
        catch (Exception ex)
        {
            return BaseResponse<BookResponse>.ErrorResponse($"Ошибка при загрузке книги: {ex.Message}");
        }
    }

    public async Task<BaseResponse<List<BookResponse>>> GetBooksByCourseAsync(int courseId)
    {
        try
        {
            var books = await _context.Books
                .Include(b => b.Courses)
                .Where(b => b.Courses.Any(c => c.Id == courseId))
                .OrderBy(b => b.Title)
                .ToListAsync();

            var bookResponses = books.Select(MapToBookResponse).ToList();
            return BaseResponse<List<BookResponse>>.SuccessResponse(bookResponses, "Книги курса успешно загружены");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<BookResponse>>.ErrorResponse($"Ошибка при загрузке книг курса: {ex.Message}");
        }
    }

    public async Task<BaseResponse<BookResponse>> CreateBookAsync(CreateBookRequest request)
    {
        try
        {
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Description = request.Description,
                ISBN = request.ISBN,
                PublicationYear = request.PublicationYear,
                Publisher = request.Publisher,
                Pages = request.Pages,
                Language = request.Language
            };

            // Добавляем связи с курсами
            if (request.CourseIds.Any())
            {
                var courses = await _context.Courses
                    .Where(c => request.CourseIds.Contains(c.Id))
                    .ToListAsync();
                book.Courses = courses;
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var bookResponse = MapToBookResponse(book);
            return BaseResponse<BookResponse>.SuccessResponse(bookResponse, "Книга успешно создана");
        }
        catch (Exception ex)
        {
            return BaseResponse<BookResponse>.ErrorResponse($"Ошибка при создании книги: {ex.Message}");
        }
    }

    public async Task<BaseResponse<BookResponse>> UpdateBookAsync(int id, UpdateBookRequest request)
    {
        try
        {
            var book = await _context.Books
                .Include(b => b.Courses)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return BaseResponse<BookResponse>.ErrorResponse("Книга не найдена");

            book.Title = request.Title;
            book.Author = request.Author;
            book.Description = request.Description;
            book.ISBN = request.ISBN;
            book.PublicationYear = request.PublicationYear;
            book.Publisher = request.Publisher;
            book.Pages = request.Pages;
            book.Language = request.Language;
            book.UpdatedAt = DateTime.UtcNow;

            // Обновляем связи с курсами
            if (request.CourseIds.Any())
            {
                var courses = await _context.Courses
                    .Where(c => request.CourseIds.Contains(c.Id))
                    .ToListAsync();
                book.Courses = courses;
            }
            else
            {
                book.Courses.Clear();
            }

            await _context.SaveChangesAsync();

            var bookResponse = MapToBookResponse(book);
            return BaseResponse<BookResponse>.SuccessResponse(bookResponse, "Книга успешно обновлена");
        }
        catch (Exception ex)
        {
            return BaseResponse<BookResponse>.ErrorResponse($"Ошибка при обновлении книги: {ex.Message}");
        }
    }

    public async Task<BaseResponse> DeleteBookAsync(int id)
    {
        try
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return BaseResponse.ErrorResponse("Книга не найдена");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return BaseResponse.SuccessResponse("Книга успешно удалена");
        }
        catch (Exception ex)
        {
            return BaseResponse.ErrorResponse($"Ошибка при удалении книги: {ex.Message}");
        }
    }

    public async Task<BaseResponse> AddBookToCourseAsync(int bookId, int courseId)
    {
        try
        {
            var book = await _context.Books
                .Include(b => b.Courses)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            var course = await _context.Courses.FindAsync(courseId);

            if (book == null)
                return BaseResponse.ErrorResponse("Книга не найдена");

            if (course == null)
                return BaseResponse.ErrorResponse("Курс не найден");

            if (book.Courses.Any(c => c.Id == courseId))
                return BaseResponse.ErrorResponse("Книга уже добавлена к этому курсу");

            book.Courses.Add(course);
            await _context.SaveChangesAsync();

            return BaseResponse.SuccessResponse("Книга успешно добавлена к курсу");
        }
        catch (Exception ex)
        {
            return BaseResponse.ErrorResponse($"Ошибка при добавлении книги к курсу: {ex.Message}");
        }
    }

    public async Task<BaseResponse> RemoveBookFromCourseAsync(int bookId, int courseId)
    {
        try
        {
            var book = await _context.Books
                .Include(b => b.Courses)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
                return BaseResponse.ErrorResponse("Книга не найдена");

            var course = book.Courses.FirstOrDefault(c => c.Id == courseId);
            if (course == null)
                return BaseResponse.ErrorResponse("Книга не связана с этим курсом");

            book.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return BaseResponse.SuccessResponse("Книга успешно удалена из курса");
        }
        catch (Exception ex)
        {
            return BaseResponse.ErrorResponse($"Ошибка при удалении книги из курса: {ex.Message}");
        }
    }

    private static BookResponse MapToBookResponse(Book book)
    {
        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            ISBN = book.ISBN,
            PublicationYear = book.PublicationYear,
            Publisher = book.Publisher,
            Pages = book.Pages,
            Language = book.Language,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt,
            Courses = book.Courses.Select(c => new CourseResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList()
        };
    }
} 