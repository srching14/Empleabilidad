using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursePlatform.Application.DTOs;
using OnlineCoursePlatform.Domain.Interfaces;

namespace OnlineCoursePlatform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetByCourse(Guid courseId)
    {
        var lessons = await _lessonService.GetByCourseIdAsync(courseId);
        return Ok(lessons);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLessonDto model)
    {
        var lesson = await _lessonService.CreateAsync(model.CourseId, model.Title, model.Order);
        return Ok(lesson);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLessonDto model)
    {
        var lesson = await _lessonService.UpdateAsync(id, model.Title);
        return Ok(lesson);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _lessonService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/reorder")]
    public async Task<IActionResult> Reorder(Guid id, [FromBody] ReorderLessonDto model)
    {
        var lesson = await _lessonService.ReorderAsync(id, model.NewOrder);
        return Ok(lesson);
    }
}
