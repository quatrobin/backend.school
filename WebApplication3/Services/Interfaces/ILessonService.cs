using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;

namespace WebApplication3.Services.Interfaces;

public interface ILessonService
{
    Task<BaseResponse<List<LessonResponse>>> GetAllLessonsAsync();
    Task<BaseResponse<List<LessonResponse>>> GetLessonsByCourseAsync(int courseId);
    Task<BaseResponse<LessonResponse>> GetLessonByIdAsync(int id);
    Task<BaseResponse<LessonResponse>> CreateLessonAsync(CreateLessonRequest request);
    Task<BaseResponse<LessonResponse>> UpdateLessonAsync(int id, UpdateLessonRequest request);
    Task<BaseResponse> DeleteLessonAsync(int id);
} 