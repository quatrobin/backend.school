using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models.Common;
using WebApplication3.Models.Requests;
using WebApplication3.Models.Responses;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonController : BaseController
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    [Authorize]
    public async Task<BaseResponse<List<LessonResponse>>> GetAllLessons()
    {
        return await _lessonService.GetAllLessonsAsync();
    }

    [HttpGet("course/{courseId}")]
    [Authorize]
    public async Task<BaseResponse<List<LessonResponse>>> GetLessonsByCourse(int courseId)
    {
        return await _lessonService.GetLessonsByCourseAsync(courseId);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<BaseResponse<LessonResponse>> GetLessonById(int id)
    {
        return await _lessonService.GetLessonByIdAsync(id);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<LessonResponse>> CreateLesson([FromBody] CreateLessonRequest request)
    {
        return await _lessonService.CreateLessonAsync(request);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse<LessonResponse>> UpdateLesson(int id, [FromBody] UpdateLessonRequest request)
    {
        return await _lessonService.UpdateLessonAsync(id, request);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    public async Task<BaseResponse> DeleteLesson(int id)
    {
        return await _lessonService.DeleteLessonAsync(id);
    }
} 