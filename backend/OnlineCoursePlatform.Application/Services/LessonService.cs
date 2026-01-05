using OnlineCoursePlatform.Application.Exceptions;
using OnlineCoursePlatform.Domain.Entities;
using OnlineCoursePlatform.Domain.Interfaces;

namespace OnlineCoursePlatform.Application.Services;

public class LessonService : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;

    public LessonService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Lesson?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
        return await _unitOfWork.Lessons.GetByIdAsync(id, includeDeleted);
    }

    public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId)
    {
        return await _unitOfWork.Lessons.GetByCourseIdAsync(courseId);
    }

    public async Task<Lesson> CreateAsync(Guid courseId, string title, int order)
    {
        var lessons = await _unitOfWork.Lessons.GetByCourseIdAsync(courseId);
        if (lessons.Any(l => l.Order == order))
        {
            throw new BusinessRuleException($"A lesson with order {order} already exists in this course");
        }

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = title,
            Order = order,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _unitOfWork.Lessons.Add(lesson);
        await _unitOfWork.SaveChangesAsync();

        return lesson;
    }

    public async Task<Lesson> UpdateAsync(Guid id, string title)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null)
        {
            throw new NotFoundException($"Lesson with ID {id} not found");
        }

        lesson.Title = title;
        lesson.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Lessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync();

        return lesson;
    }

    public async Task DeleteAsync(Guid id)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null)
        {
            throw new NotFoundException($"Lesson with ID {id} not found");
        }

        lesson.IsDeleted = true;
        lesson.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Lessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Lesson> ReorderAsync(Guid id, int newOrder)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null)
        {
            throw new NotFoundException($"Lesson with ID {id} not found");
        }

        if (lesson.Order == newOrder) return lesson;

        var lessons = await _unitOfWork.Lessons.GetByCourseIdAsync(lesson.CourseId);
        var conflictingLesson = lessons.FirstOrDefault(l => l.Order == newOrder);

        if (conflictingLesson != null)
        {
            var oldOrder = lesson.Order;
            lesson.Order = newOrder;
            conflictingLesson.Order = oldOrder;
            conflictingLesson.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Lessons.Update(conflictingLesson);
        }
        else
        {
            lesson.Order = newOrder;
        }

        lesson.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Lessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync();

        return lesson;
    }
}
