using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursePlatform.Domain.Enums;
using OnlineCoursePlatform.Domain.Interfaces;

namespace OnlineCoursePlatform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? query, [FromQuery] CourseStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _courseService.SearchAsync(query, status, page, pageSize);
        return Ok(new { items = result.Courses, totalCount = result.TotalCount });
    }

    [HttpGet("{id}/summary")]
    public async Task<IActionResult> GetSummary(Guid id)
    {
        return Ok(await _courseService.GetSummaryAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseModel model)
    {
        var course = await _courseService.CreateAsync(model.Title, model.Description);
        return CreatedAtAction(nameof(GetSummary), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateCourseModel model)
    {
        var course = await _courseService.UpdateAsync(id, model.Title, model.Description);
        return Ok(course);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _courseService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        var course = await _courseService.PublishAsync(id);
        return Ok(course);
    }

    [HttpPost("{id}/unpublish")]
    public async Task<IActionResult> Unpublish(Guid id)
    {
        var course = await _courseService.UnpublishAsync(id);
        return Ok(course);
    }
}

public class CreateCourseModel 
{ 
    public string Title { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;
}
