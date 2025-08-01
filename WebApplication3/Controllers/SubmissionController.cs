using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubmissionController : BaseController
{
    private readonly ISubmissionService _submissionService;

    public SubmissionController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet("assignment/{assignmentId}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<List<SubmissionResponse>>> GetSubmissionsByAssignment(int assignmentId)
    {
        return await _submissionService.GetSubmissionsByAssignmentAsync(assignmentId);
    }

    [HttpGet("student/{studentId}")]
    [Authorize]
    public async Task<BaseResponse<List<SubmissionResponse>>> GetSubmissionsByStudent(int studentId)
    {
        // Проверяем, что студент запрашивает свои отправки
        var currentUserId = GetCurrentUserId();
        if (currentUserId != studentId)
        {
            return new BaseResponse<List<SubmissionResponse>>
            {
                Success = false,
                Message = "Доступ запрещен"
            };
        }

        return await _submissionService.GetSubmissionsByStudentAsync(studentId);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<BaseResponse<SubmissionResponse>> GetSubmissionById(int id)
    {
        return await _submissionService.GetSubmissionByIdAsync(id);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Student)]
    public async Task<BaseResponse<SubmissionResponse>> CreateSubmission([FromBody] CreateSubmissionRequest request)
    {
        var studentId = GetCurrentUserId();
        return await _submissionService.CreateSubmissionAsync(request, studentId);
    }

    [HttpPost("{submissionId}/grade")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<SubmissionResponse>> GradeSubmission(int submissionId, [FromBody] GradeSubmissionRequest request)
    {
        var teacherId = GetCurrentUserId();
        return await _submissionService.GradeSubmissionAsync(submissionId, request, teacherId);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse> DeleteSubmission(int id)
    {
        return await _submissionService.DeleteSubmissionAsync(id);
    }
} 