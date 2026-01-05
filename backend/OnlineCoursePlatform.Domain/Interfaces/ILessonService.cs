using OnlineCoursePlatform.Domain.Entities;

namespace OnlineCoursePlatform.Domain.Interfaces;

public interface ILessonService
{
    Task<Lesson?> GetByIdAsync(Guid id, bool includeDeleted = false);
    Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId);
    Task<Lesson> CreateAsync(Guid courseId, string title, int order);
    Task<Lesson> UpdateAsync(Guid id, string title);
    Task DeleteAsync(Guid id);
    Task<Lesson> ReorderAsync(Guid id, int newOrder);
}
