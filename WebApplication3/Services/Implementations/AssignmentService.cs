using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Models.Entities;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Services.Implementations;

public class AssignmentService : IAssignmentService
{
    private readonly ApplicationDbContext _context;

    public AssignmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<AssignmentResponse>>> GetAllAssignmentsAsync()
    {
        try
        {
            var assignments = await _context.Assignments
                .Include(a => a.Course)
                .OrderBy(a => a.DueDate)
                .Select(a => new AssignmentResponse
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    CourseId = a.CourseId,
                    CourseName = a.Course.Name,
                    DueDate = a.DueDate,
                    MaxScore = a.MaxScore,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();

            return new BaseResponse<List<AssignmentResponse>>
            {
                Success = true,
                Data = assignments,
                Message = "Задания успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<AssignmentResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке заданий: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<List<AssignmentResponse>>> GetAssignmentsByCourseAsync(int courseId)
    {
        try
        {
            var assignments = await _context.Assignments
                .Include(a => a.Course)
                .Where(a => a.CourseId == courseId)
                .OrderBy(a => a.DueDate)
                .Select(a => new AssignmentResponse
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    CourseId = a.CourseId,
                    CourseName = a.Course.Name,
                    DueDate = a.DueDate,
                    MaxScore = a.MaxScore,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();

            return new BaseResponse<List<AssignmentResponse>>
            {
                Success = true,
                Data = assignments,
                Message = "Задания курса успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<AssignmentResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке заданий курса: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<List<AssignmentResponse>>> GetMyAssignmentsAsync(int userId)
    {
        try
        {
            // Получаем курсы, на которые записан пользователь
            var userCourses = await _context.CourseEnrollments
                .Where(ce => ce.StudentId == userId)
                .Select(ce => ce.CourseId)
                .ToListAsync();

            if (!userCourses.Any())
            {
                return new BaseResponse<List<AssignmentResponse>>
                {
                    Success = true,
                    Data = new List<AssignmentResponse>(),
                    Message = "У вас нет записей на курсы"
                };
            }

            // Получаем задания для курсов пользователя
            var assignments = await _context.Assignments
                .Include(a => a.Course)
                .Where(a => userCourses.Contains(a.CourseId))
                .OrderBy(a => a.DueDate)
                .Select(a => new AssignmentResponse
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    CourseId = a.CourseId,
                    CourseName = a.Course.Name,
                    DueDate = a.DueDate,
                    MaxScore = a.MaxScore,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();

            return new BaseResponse<List<AssignmentResponse>>
            {
                Success = true,
                Data = assignments,
                Message = "Ваши задания успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<AssignmentResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке ваших заданий: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<AssignmentResponse>> GetAssignmentByIdAsync(int id)
    {
        try
        {
            var assignment = await _context.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return new BaseResponse<AssignmentResponse>
                {
                    Success = false,
                    Message = "Задание не найдено"
                };
            }

            var response = new AssignmentResponse
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                CourseId = assignment.CourseId,
                CourseName = assignment.Course.Name,
                DueDate = assignment.DueDate,
                MaxScore = assignment.MaxScore,
                CreatedAt = assignment.CreatedAt,
                UpdatedAt = assignment.UpdatedAt
            };

            return new BaseResponse<AssignmentResponse>
            {
                Success = true,
                Data = response,
                Message = "Задание успешно загружено"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<AssignmentResponse>
            {
                Success = false,
                Message = $"Ошибка при загрузке задания: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<AssignmentResponse>> CreateAssignmentAsync(CreateAssignmentRequest request)
    {
        try
        {
            var course = await _context.Courses.FindAsync(request.CourseId);
            if (course == null)
            {
                return new BaseResponse<AssignmentResponse>
                {
                    Success = false,
                    Message = "Курс не найден"
                };
            }

            var assignment = new Assignment
            {
                Title = request.Title,
                Description = request.Description,
                CourseId = request.CourseId,
                DueDate = request.DueDate,
                MaxScore = request.MaxScore,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            var response = new AssignmentResponse
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                CourseId = assignment.CourseId,
                CourseName = course.Name,
                DueDate = assignment.DueDate,
                MaxScore = assignment.MaxScore,
                CreatedAt = assignment.CreatedAt,
                UpdatedAt = assignment.UpdatedAt
            };

            return new BaseResponse<AssignmentResponse>
            {
                Success = true,
                Data = response,
                Message = "Задание успешно создано"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<AssignmentResponse>
            {
                Success = false,
                Message = $"Ошибка при создании задания: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<AssignmentResponse>> UpdateAssignmentAsync(int id, CreateAssignmentRequest request)
    {
        try
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return new BaseResponse<AssignmentResponse>
                {
                    Success = false,
                    Message = "Задание не найдено"
                };
            }

            var course = await _context.Courses.FindAsync(request.CourseId);
            if (course == null)
            {
                return new BaseResponse<AssignmentResponse>
                {
                    Success = false,
                    Message = "Курс не найден"
                };
            }

            assignment.Title = request.Title;
            assignment.Description = request.Description;
            assignment.CourseId = request.CourseId;
            assignment.DueDate = request.DueDate;
            assignment.MaxScore = request.MaxScore;
            assignment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new AssignmentResponse
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                CourseId = assignment.CourseId,
                CourseName = course.Name,
                DueDate = assignment.DueDate,
                MaxScore = assignment.MaxScore,
                CreatedAt = assignment.CreatedAt,
                UpdatedAt = assignment.UpdatedAt
            };

            return new BaseResponse<AssignmentResponse>
            {
                Success = true,
                Data = response,
                Message = "Задание успешно обновлено"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<AssignmentResponse>
            {
                Success = false,
                Message = $"Ошибка при обновлении задания: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse> DeleteAssignmentAsync(int id)
    {
        try
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Задание не найдено"
                };
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Задание успешно удалено"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse
            {
                Success = false,
                Message = $"Ошибка при удалении задания: {ex.Message}"
            };
        }
    }
} 