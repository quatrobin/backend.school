using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;

namespace WebApplication3.Services.Interfaces;

public interface ISubmissionService
{
    Task<BaseResponse<List<SubmissionResponse>>> GetSubmissionsByAssignmentAsync(int assignmentId);
    Task<BaseResponse<List<SubmissionResponse>>> GetSubmissionsByStudentAsync(int studentId);
    Task<BaseResponse<SubmissionResponse>> GetSubmissionByIdAsync(int id);
    Task<BaseResponse<SubmissionResponse>> CreateSubmissionAsync(CreateSubmissionRequest request, int studentId);
    Task<BaseResponse<SubmissionResponse>> GradeSubmissionAsync(int submissionId, GradeSubmissionRequest request, int teacherId);
    Task<BaseResponse> DeleteSubmissionAsync(int id);
} 