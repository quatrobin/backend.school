using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Models.Entities;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Services.Implementations;

public class LessonService : ILessonService
{
    private readonly ApplicationDbContext _context;

    public LessonService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<LessonResponse>>> GetAllLessonsAsync()
    {
        try
        {
            var lessons = await _context.Lessons
                .Include(l => l.Course)
                .OrderBy(l => l.LessonDate)
                .Select(l => new LessonResponse
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    CourseId = l.CourseId,
                    CourseName = l.Course.Name,
                    LessonDate = l.LessonDate,
                    DurationMinutes = l.DurationMinutes,
                    Materials = l.Materials,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                })
                .ToListAsync();

            return new BaseResponse<List<LessonResponse>>
            {
                Success = true,
                Data = lessons,
                Message = "Уроки успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<LessonResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке уроков: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<List<LessonResponse>>> GetLessonsByCourseAsync(int courseId)
    {
        try
        {
            var lessons = await _context.Lessons
                .Include(l => l.Course)
                .Where(l => l.CourseId == courseId)
                .OrderBy(l => l.LessonDate)
                .Select(l => new LessonResponse
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    CourseId = l.CourseId,
                    CourseName = l.Course.Name,
                    LessonDate = l.LessonDate,
                    DurationMinutes = l.DurationMinutes,
                    Materials = l.Materials,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                })
                .ToListAsync();

            return new BaseResponse<List<LessonResponse>>
            {
                Success = true,
                Data = lessons,
                Message = "Уроки курса успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<LessonResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке уроков курса: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<LessonResponse>> GetLessonByIdAsync(int id)
    {
        try
        {
            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
            {
                return new BaseResponse<LessonResponse>
                {
                    Success = false,
                    Message = "Урок не найден"
                };
            }

            var response = new LessonResponse
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Description = lesson.Description,
                CourseId = lesson.CourseId,
                CourseName = lesson.Course.Name,
                LessonDate = lesson.LessonDate,
                DurationMinutes = lesson.DurationMinutes,
                Materials = lesson.Materials,
                CreatedAt = lesson.CreatedAt,
                UpdatedAt = lesson.UpdatedAt
            };

            return new BaseResponse<LessonResponse>
            {
                Success = true,
                Data = response,
                Message = "Урок успешно загружен"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<LessonResponse>
            {
                Success = false,
                Message = $"Ошибка при загрузке урока: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<LessonResponse>> CreateLessonAsync(CreateLessonRequest request)
    {
        try
        {
            var course = await _context.Courses.FindAsync(request.CourseId);
            if (course == null)
            {
                return new BaseResponse<LessonResponse>
                {
                    Success = false,
                    Message = "Курс не найден"
                };
            }

            var lesson = new Lesson
            {
                Title = request.Title,
                Description = request.Description,
                CourseId = request.CourseId,
                LessonDate = request.LessonDate,
                DurationMinutes = request.DurationMinutes,
                Materials = request.Materials,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            var response = new LessonResponse
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Description = lesson.Description,
                CourseId = lesson.CourseId,
                CourseName = course.Name,
                LessonDate = lesson.LessonDate,
                DurationMinutes = lesson.DurationMinutes,
                Materials = lesson.Materials,
                CreatedAt = lesson.CreatedAt,
                UpdatedAt = lesson.UpdatedAt
            };

            return new BaseResponse<LessonResponse>
            {
                Success = true,
                Data = response,
                Message = "Урок успешно создан"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<LessonResponse>
            {
                Success = false,
                Message = $"Ошибка при создании урока: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<LessonResponse>> UpdateLessonAsync(int id, UpdateLessonRequest request)
    {
        try
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return new BaseResponse<LessonResponse>
                {
                    Success = false,
                    Message = "Урок не найден"
                };
            }

            var course = await _context.Courses.FindAsync(request.CourseId);
            if (course == null)
            {
                return new BaseResponse<LessonResponse>
                {
                    Success = false,
                    Message = "Курс не найден"
                };
            }

            lesson.Title = request.Title;
            lesson.Description = request.Description;
            lesson.CourseId = request.CourseId;
            lesson.LessonDate = request.LessonDate;
            lesson.DurationMinutes = request.DurationMinutes;
            lesson.Materials = request.Materials;
            lesson.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new LessonResponse
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Description = lesson.Description,
                CourseId = lesson.CourseId,
                CourseName = course.Name,
                LessonDate = lesson.LessonDate,
                DurationMinutes = lesson.DurationMinutes,
                Materials = lesson.Materials,
                CreatedAt = lesson.CreatedAt,
                UpdatedAt = lesson.UpdatedAt
            };

            return new BaseResponse<LessonResponse>
            {
                Success = true,
                Data = response,
                Message = "Урок успешно обновлен"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<LessonResponse>
            {
                Success = false,
                Message = $"Ошибка при обновлении урока: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse> DeleteLessonAsync(int id)
    {
        try
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Урок не найден"
                };
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Урок успешно удален"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse
            {
                Success = false,
                Message = $"Ошибка при удалении урока: {ex.Message}"
            };
        }
    }
} 