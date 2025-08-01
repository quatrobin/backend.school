using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;

namespace WebApplication3.Services.Interfaces;

public interface IBookService
{
    Task<BaseResponse<List<BookResponse>>> GetAllBooksAsync();
    Task<BaseResponse<BookResponse>> GetBookByIdAsync(int id);
    Task<BaseResponse<List<BookResponse>>> GetBooksByCourseAsync(int courseId);
    Task<BaseResponse<BookResponse>> CreateBookAsync(CreateBookRequest request);
    Task<BaseResponse<BookResponse>> UpdateBookAsync(int id, UpdateBookRequest request);
    Task<BaseResponse> DeleteBookAsync(int id);
    Task<BaseResponse> AddBookToCourseAsync(int bookId, int courseId);
    Task<BaseResponse> RemoveBookFromCourseAsync(int bookId, int courseId);
} 