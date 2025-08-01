using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : BaseController
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    [Authorize]
    public async Task<BaseResponse<List<CourseResponse>>> GetAllCourses()
    {
        return await _courseService.GetAllCoursesAsync();
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<BaseResponse<CourseResponse>> GetCourseById(int id)
    {
        return await _courseService.GetCourseByIdAsync(id);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<CourseResponse>> CreateCourse([FromBody] CreateCourseRequest request)
    {
        return await _courseService.CreateCourseAsync(request);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<CourseResponse>> UpdateCourse(int id, [FromBody] CreateCourseRequest request)
    {
        return await _courseService.UpdateCourseAsync(id, request);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse> DeleteCourse(int id)
    {
        return await _courseService.DeleteCourseAsync(id);
    }

    [HttpPost("{courseId}/enroll")]
    [Authorize(Roles = Roles.Student)]
    public async Task<BaseResponse> EnrollInCourse(int courseId)
    {
        var studentId = GetCurrentUserId();
        return await _courseService.EnrollStudentAsync(courseId, studentId);
    }

    [HttpDelete("{courseId}/unenroll")]
    [Authorize(Roles = Roles.Student)]
    public async Task<BaseResponse> UnenrollFromCourse(int courseId)
    {
        var studentId = GetCurrentUserId();
        return await _courseService.UnenrollStudentAsync(courseId, studentId);
    }
} 