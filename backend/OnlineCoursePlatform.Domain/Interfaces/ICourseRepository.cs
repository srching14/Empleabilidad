using OnlineCoursePlatform.Domain.Entities;
using OnlineCoursePlatform.Domain.Enums;

namespace OnlineCoursePlatform.Domain.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id, bool includeDeleted = false);
    Task<(IEnumerable<Course> Courses, int TotalCount)> SearchAsync(string? searchQuery, CourseStatus? status, int page, int pageSize);
    void Add(Course course);
    void Update(Course course);
}
