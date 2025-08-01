using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;

namespace WebApplication3.Services.Interfaces;

public interface ICourseService
{
    Task<BaseResponse<List<CourseResponse>>> GetAllCoursesAsync();
    Task<BaseResponse<CourseResponse>> GetCourseByIdAsync(int id);
    Task<BaseResponse<CourseResponse>> CreateCourseAsync(CreateCourseRequest request);
    Task<BaseResponse<CourseResponse>> UpdateCourseAsync(int id, CreateCourseRequest request);
    Task<BaseResponse> DeleteCourseAsync(int id);
    Task<BaseResponse> EnrollStudentAsync(int courseId, int studentId);
    Task<BaseResponse> UnenrollStudentAsync(int courseId, int studentId);
} 