using OnlineCoursePlatform.Application.Exceptions;
using OnlineCoursePlatform.Domain.Entities;
using OnlineCoursePlatform.Domain.Enums;
using OnlineCoursePlatform.Domain.Interfaces;

namespace OnlineCoursePlatform.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Course?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
        return await _unitOfWork.Courses.GetByIdAsync(id, includeDeleted);
    }

    public async Task<(IEnumerable<Course> Courses, int TotalCount)> SearchAsync(
        string? searchQuery, 
        CourseStatus? status, 
        int page, 
        int pageSize)
    {
        return await _unitOfWork.Courses.SearchAsync(searchQuery, status, page, pageSize);
    }

    public async Task<Course> CreateAsync(string title, string description)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Status = CourseStatus.Draft,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _unitOfWork.Courses.Add(course);
        await _unitOfWork.SaveChangesAsync();

        return course;
    }

    public async Task<Course> UpdateAsync(Guid id, string title, string description)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            throw new NotFoundException($"Course with ID {id} not found");
        }

        course.Title = title;
        course.Description = description;
        course.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        return course;
    }

    public async Task DeleteAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            throw new NotFoundException($"Course with ID {id} not found");
        }

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Course> PublishAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            throw new NotFoundException($"Course with ID {id} not found");
        }

        var activeLessonsCount = course.Lessons.Count(l => !l.IsDeleted);
        if (activeLessonsCount == 0)
        {
            throw new BusinessRuleException("Cannot publish a course without active lessons");
        }

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        return course;
    }

    public async Task<Course> UnpublishAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            throw new NotFoundException($"Course with ID {id} not found");
        }

        course.Status = CourseStatus.Draft;
        course.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        return course;
    }

    public async Task<object> GetSummaryAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            throw new NotFoundException($"Course with ID {id} not found");
        }

        return new
        {
            course.Id,
            course.Title,
            course.Status,
            TotalLessons = course.Lessons.Count(l => !l.IsDeleted),
            LastModifiedAt = course.UpdatedAt
        };
    }
}
