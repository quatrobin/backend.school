using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Models.Entities;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<CourseResponse>>> GetAllCoursesAsync()
    {
        try
        {
            var courses = await _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .Select(c => new CourseResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TeacherName = "Преподаватель", // Пока заглушка
                    StudentsCount = c.Enrollments.Count,
                    LessonsCount = c.Lessons.Count,
                    IsActive = true,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();

            return new BaseResponse<List<CourseResponse>>
            {
                Success = true,
                Data = courses,
                Message = "Курсы успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<CourseResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке курсов: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<CourseResponse>> GetCourseByIdAsync(int id)
    {
        try
        {
            var course = await _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return new BaseResponse<CourseResponse>
                {
                    Success = false,
                    Message = "Курс не найден"
                };
            }

            var response = new CourseResponse
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                TeacherName = "Преподаватель", // Пока заглушка
                StudentsCount = course.Enrollments.Count,
                LessonsCount = course.Lessons.Count,
                IsActive = true,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };

            return new BaseResponse<CourseResponse>
            {
                Success = true,
                Data = response,
                Message = "Курс успешно загружен"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<CourseResponse>
            {
                Success = false,
                Message = $"Ошибка при загрузке курса: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<CourseResponse>> CreateCourseAsync(CreateCourseRequest request)
    {
        try
        {
            var course = new Course
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var response = new CourseResponse
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                TeacherName = "Преподаватель",
                StudentsCount = 0,
                LessonsCount = 0,
                IsActive = true,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };

            return new BaseResponse<CourseResponse>
            {
                Success = true,
                Data = response,
                Message = "Курс успешно создан"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<CourseResponse>
            {
                Success = false,
                Message = $"Ошибка при создании курса: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<CourseResponse>> UpdateCourseAsync(int id, CreateCourseRequest request)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return new BaseResponse<CourseResponse>
                {
                    Success = false,
                    Message = "Курс не найден"
                };
            }

            course.Name = request.Name;
            course.Description = request.Description;
            course.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new CourseResponse
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                TeacherName = "Преподаватель",
                StudentsCount = 0,
                LessonsCount = 0,
                IsActive = true,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };

            return new BaseResponse<CourseResponse>
            {
                Success = true,
                Data = response,
                Message = "Курс успешно обновлен"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<CourseResponse>
            {
                Success = false,
                Message = $"Ошибка при обновлении курса: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse> DeleteCourseAsync(int id)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Курс не найден"
                };
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Курс успешно удален"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse
            {
                Success = false,
                Message = $"Ошибка при удалении курса: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse> EnrollStudentAsync(int courseId, int studentId)
    {
        try
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Курс не найден"
                };
            }

            var student = await _context.Users.FindAsync(studentId);
            if (student == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Студент не найден"
                };
            }

            var existingEnrollment = await _context.CourseEnrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);

            if (existingEnrollment != null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Студент уже записан на этот курс"
                };
            }

            var enrollment = new CourseEnrollment
            {
                CourseId = courseId,
                StudentId = studentId,
                EnrolledAt = DateTime.UtcNow
            };

            _context.CourseEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Студент успешно записан на курс"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse
            {
                Success = false,
                Message = $"Ошибка при записи на курс: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse> UnenrollStudentAsync(int courseId, int studentId)
    {
        try
        {
            var enrollment = await _context.CourseEnrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);

            if (enrollment == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Запись на курс не найдена"
                };
            }

            _context.CourseEnrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Студент успешно отписан от курса"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse
            {
                Success = false,
                Message = $"Ошибка при отписке от курса: {ex.Message}"
            };
        }
    }
} 