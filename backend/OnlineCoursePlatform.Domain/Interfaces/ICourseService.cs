using OnlineCoursePlatform.Domain.Entities;

namespace OnlineCoursePlatform.Domain.Interfaces;

public interface ICourseService
{
    Task<Course?> GetByIdAsync(Guid id, bool includeDeleted = false);
    Task<(IEnumerable<Course> Courses, int TotalCount)> SearchAsync(string? searchQuery, Enums.CourseStatus? status, int page, int pageSize);
    Task<Course> CreateAsync(string title);
    Task<Course> UpdateAsync(Guid id, string title);
    Task DeleteAsync(Guid id);
    Task<Course> PublishAsync(Guid id);
    Task<Course> UnpublishAsync(Guid id);
    Task<object> GetSummaryAsync(Guid id);
}
