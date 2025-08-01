using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Models.Entities;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Services.Implementations;

public class SubmissionService : ISubmissionService
{
    private readonly ApplicationDbContext _context;

    public SubmissionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<SubmissionResponse>>> GetSubmissionsByAssignmentAsync(int assignmentId)
    {
        try
        {
            var submissions = await _context.AssignmentSubmissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Include(s => s.Grader)
                .Where(s => s.AssignmentId == assignmentId)
                .OrderBy(s => s.SubmittedAt)
                .Select(s => new SubmissionResponse
                {
                    Id = s.Id,
                    AssignmentId = s.AssignmentId,
                    AssignmentTitle = s.Assignment.Title,
                    StudentId = s.StudentId,
                    StudentName = $"{s.Student.FirstName} {s.Student.LastName}",
                    Content = s.Content,
                    FileUrl = s.FileUrl,
                    SubmittedAt = s.SubmittedAt,
                    Score = s.Score,
                    Feedback = s.Feedback,
                    GradedAt = s.GradedAt,
                    GradedBy = s.GradedBy,
                    GraderName = s.Grader != null ? $"{s.Grader.FirstName} {s.Grader.LastName}" : null
                })
                .ToListAsync();

            return new BaseResponse<List<SubmissionResponse>>
            {
                Success = true,
                Data = submissions,
                Message = "Отправки успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<SubmissionResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке отправок: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<List<SubmissionResponse>>> GetSubmissionsByStudentAsync(int studentId)
    {
        try
        {
            var submissions = await _context.AssignmentSubmissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Include(s => s.Grader)
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.SubmittedAt)
                .Select(s => new SubmissionResponse
                {
                    Id = s.Id,
                    AssignmentId = s.AssignmentId,
                    AssignmentTitle = s.Assignment.Title,
                    StudentId = s.StudentId,
                    StudentName = $"{s.Student.FirstName} {s.Student.LastName}",
                    Content = s.Content,
                    FileUrl = s.FileUrl,
                    SubmittedAt = s.SubmittedAt,
                    Score = s.Score,
                    Feedback = s.Feedback,
                    GradedAt = s.GradedAt,
                    GradedBy = s.GradedBy,
                    GraderName = s.Grader != null ? $"{s.Grader.FirstName} {s.Grader.LastName}" : null
                })
                .ToListAsync();

            return new BaseResponse<List<SubmissionResponse>>
            {
                Success = true,
                Data = submissions,
                Message = "Отправки студента успешно загружены"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<SubmissionResponse>>
            {
                Success = false,
                Message = $"Ошибка при загрузке отправок студента: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<SubmissionResponse>> GetSubmissionByIdAsync(int id)
    {
        try
        {
            var submission = await _context.AssignmentSubmissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Include(s => s.Grader)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (submission == null)
            {
                return new BaseResponse<SubmissionResponse>
                {
                    Success = false,
                    Message = "Отправка не найдена"
                };
            }

            var response = new SubmissionResponse
            {
                Id = submission.Id,
                AssignmentId = submission.AssignmentId,
                AssignmentTitle = submission.Assignment.Title,
                StudentId = submission.StudentId,
                StudentName = $"{submission.Student.FirstName} {submission.Student.LastName}",
                Content = submission.Content,
                FileUrl = submission.FileUrl,
                SubmittedAt = submission.SubmittedAt,
                Score = submission.Score,
                Feedback = submission.Feedback,
                GradedAt = submission.GradedAt,
                GradedBy = submission.GradedBy,
                GraderName = submission.Grader != null ? $"{submission.Grader.FirstName} {submission.Grader.LastName}" : null
            };

            return new BaseResponse<SubmissionResponse>
            {
                Success = true,
                Data = response,
                Message = "Отправка успешно загружена"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<SubmissionResponse>
            {
                Success = false,
                Message = $"Ошибка при загрузке отправки: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<SubmissionResponse>> CreateSubmissionAsync(CreateSubmissionRequest request, int studentId)
    {
        try
        {
            var assignment = await _context.Assignments.FindAsync(request.AssignmentId);
            if (assignment == null)
            {
                return new BaseResponse<SubmissionResponse>
                {
                    Success = false,
                    Message = "Задание не найдено"
                };
            }

            var student = await _context.Users.FindAsync(studentId);
            if (student == null)
            {
                return new BaseResponse<SubmissionResponse>
                {
                    Success = false,
                    Message = "Студент не найден"
                };
            }

            // Проверяем, не отправлял ли уже студент это задание
            var existingSubmission = await _context.AssignmentSubmissions
                .FirstOrDefaultAsync(s => s.AssignmentId == request.AssignmentId && s.StudentId == studentId);

            if (existingSubmission != null)
            {
                return new BaseResponse<SubmissionResponse>
                {
                    Success = false,
                    Message = "Вы уже отправили это задание"
                };
            }

            var submission = new AssignmentSubmission
            {
                AssignmentId = request.AssignmentId,
                StudentId = studentId,
                Content = request.Content,
                FileUrl = request.FileUrl,
                SubmittedAt = DateTime.UtcNow
            };

            _context.AssignmentSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            var response = new SubmissionResponse
            {
                Id = submission.Id,
                AssignmentId = submission.AssignmentId,
                AssignmentTitle = assignment.Title,
                StudentId = submission.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                Content = submission.Content,
                FileUrl = submission.FileUrl,
                SubmittedAt = submission.SubmittedAt,
                Score = submission.Score,
                Feedback = submission.Feedback,
                GradedAt = submission.GradedAt,
                GradedBy = submission.GradedBy,
                GraderName = null
            };

            return new BaseResponse<SubmissionResponse>
            {
                Success = true,
                Data = response,
                Message = "Задание успешно отправлено"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<SubmissionResponse>
            {
                Success = false,
                Message = $"Ошибка при отправке задания: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse<SubmissionResponse>> GradeSubmissionAsync(int submissionId, GradeSubmissionRequest request, int teacherId)
    {
        try
        {
            var submission = await _context.AssignmentSubmissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null)
            {
                return new BaseResponse<SubmissionResponse>
                {
                    Success = false,
                    Message = "Отправка не найдена"
                };
            }

            var teacher = await _context.Users.FindAsync(teacherId);
            if (teacher == null)
            {
                return new BaseResponse<SubmissionResponse>
                {
                    Success = false,
                    Message = "Преподаватель не найден"
                };
            }

            submission.Score = request.Score;
            submission.Feedback = request.Feedback;
            submission.GradedAt = DateTime.UtcNow;
            submission.GradedBy = teacherId;

            await _context.SaveChangesAsync();

            var response = new SubmissionResponse
            {
                Id = submission.Id,
                AssignmentId = submission.AssignmentId,
                AssignmentTitle = submission.Assignment.Title,
                StudentId = submission.StudentId,
                StudentName = $"{submission.Student.FirstName} {submission.Student.LastName}",
                Content = submission.Content,
                FileUrl = submission.FileUrl,
                SubmittedAt = submission.SubmittedAt,
                Score = submission.Score,
                Feedback = submission.Feedback,
                GradedAt = submission.GradedAt,
                GradedBy = submission.GradedBy,
                GraderName = $"{teacher.FirstName} {teacher.LastName}"
            };

            return new BaseResponse<SubmissionResponse>
            {
                Success = true,
                Data = response,
                Message = "Задание успешно оценено"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<SubmissionResponse>
            {
                Success = false,
                Message = $"Ошибка при оценке задания: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponse> DeleteSubmissionAsync(int id)
    {
        try
        {
            var submission = await _context.AssignmentSubmissions.FindAsync(id);
            if (submission == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Отправка не найдена"
                };
            }

            _context.AssignmentSubmissions.Remove(submission);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Отправка успешно удалена"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse
            {
                Success = false,
                Message = $"Ошибка при удалении отправки: {ex.Message}"
            };
        }
    }
} 