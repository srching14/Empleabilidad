using OnlineCoursePlatform.Domain.Entities;

namespace OnlineCoursePlatform.Domain.Interfaces;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(Guid id, bool includeDeleted = false);
    Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId);
    void Add(Lesson lesson);
    void Update(Lesson lesson);
}
