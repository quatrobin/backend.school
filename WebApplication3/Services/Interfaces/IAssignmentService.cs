using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;

namespace WebApplication3.Services.Interfaces;

public interface IAssignmentService
{
    Task<BaseResponse<List<AssignmentResponse>>> GetAllAssignmentsAsync();
    Task<BaseResponse<List<AssignmentResponse>>> GetAssignmentsByCourseAsync(int courseId);
    Task<BaseResponse<List<AssignmentResponse>>> GetMyAssignmentsAsync(int userId);
    Task<BaseResponse<AssignmentResponse>> GetAssignmentByIdAsync(int id);
    Task<BaseResponse<AssignmentResponse>> CreateAssignmentAsync(CreateAssignmentRequest request);
    Task<BaseResponse<AssignmentResponse>> UpdateAssignmentAsync(int id, CreateAssignmentRequest request);
    Task<BaseResponse> DeleteAssignmentAsync(int id);
} 