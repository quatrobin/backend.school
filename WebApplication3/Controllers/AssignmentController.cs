using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssignmentController : BaseController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    [Authorize]
    public async Task<BaseResponse<List<AssignmentResponse>>> GetAllAssignments()
    {
        return await _assignmentService.GetAllAssignmentsAsync();
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<BaseResponse<List<AssignmentResponse>>> GetMyAssignments()
    {
        var userId = GetCurrentUserId();
        return await _assignmentService.GetMyAssignmentsAsync(userId);
    }

    [HttpGet("course/{courseId}")]
    [Authorize]
    public async Task<BaseResponse<List<AssignmentResponse>>> GetAssignmentsByCourse(int courseId)
    {
        return await _assignmentService.GetAssignmentsByCourseAsync(courseId);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<BaseResponse<AssignmentResponse>> GetAssignmentById(int id)
    {
        return await _assignmentService.GetAssignmentByIdAsync(id);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<AssignmentResponse>> CreateAssignment([FromBody] CreateAssignmentRequest request)
    {
        return await _assignmentService.CreateAssignmentAsync(request);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<AssignmentResponse>> UpdateAssignment(int id, [FromBody] CreateAssignmentRequest request)
    {
        return await _assignmentService.UpdateAssignmentAsync(id, request);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse> DeleteAssignment(int id)
    {
        return await _assignmentService.DeleteAssignmentAsync(id);
    }
} 