using Microsoft.EntityFrameworkCore;
using OnlineCoursePlatform.Application.Exceptions;
using OnlineCoursePlatform.Application.Services;
using OnlineCoursePlatform.Domain.Interfaces;
using OnlineCoursePlatform.Infrastructure.Data;
using OnlineCoursePlatform.Infrastructure.Repositories;

namespace OnlineCoursePlatform.Tests;

public class LessonServiceTests
{
    private IUnitOfWork GetUnitOfWork()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        return new UnitOfWork(context);
    }

    [Fact]
    public async Task CreateLesson_WithUniqueOrder_ShouldSucceed()
    {
        // Arrange
        var uow = GetUnitOfWork();
        var courseService = new CourseService(uow);
        var lessonService = new LessonService(uow);

        var course = await courseService.CreateAsync("Test Course");

        // Act
        var lesson = await lessonService.CreateAsync(course.Id, "Lesson 1", 1);

        // Assert
        Assert.NotNull(lesson);
        Assert.Equal("Lesson 1", lesson.Title);
        Assert.Equal(1, lesson.Order);
        Assert.Equal(course.Id, lesson.CourseId);
    }

    [Fact]
    public async Task CreateLesson_WithDuplicateOrder_ShouldFail()
    {
        // Arrange
        var uow = GetUnitOfWork();
        var courseService = new CourseService(uow);
        var lessonService = new LessonService(uow);

        var course = await courseService.CreateAsync("Test Course");
        
        await lessonService.CreateAsync(course.Id, "Lesson 1", 1);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(
            async () => await lessonService.CreateAsync(course.Id, "Lesson 2", 1));

        Assert.Contains("already exists", exception.Message);
    }

    [Fact]
    public async Task DeleteLesson_ShouldBeSoftDelete()
    {
        // Arrange
        var uow = GetUnitOfWork();
        var lessonService = new LessonService(uow);

        var lesson = await lessonService.CreateAsync(Guid.NewGuid(), "Lesson to Delete", 1);
        var lessonId = lesson.Id;

        // Act
        await lessonService.DeleteAsync(lessonId);

        // Assert
        var deletedLesson = await lessonService.GetByIdAsync(lessonId, includeDeleted: false);
        Assert.Null(deletedLesson);

        var lessonWithDeleted = await lessonService.GetByIdAsync(lessonId, includeDeleted: true);
        Assert.NotNull(lessonWithDeleted);
        Assert.True(lessonWithDeleted.IsDeleted);
    }

    [Fact]
    public async Task ReorderLessons_ShouldSwapPositions()
    {
        // Arrange
        var uow = GetUnitOfWork();
        var lessonService = new LessonService(uow);
        var courseId = Guid.NewGuid();

        var lesson1 = await lessonService.CreateAsync(courseId, "Lesson 1", 1);
        var lesson2 = await lessonService.CreateAsync(courseId, "Lesson 2", 2);

        // Act
        await lessonService.ReorderAsync(lesson1.Id, 2);

        // Assert
        var updatedLesson1 = await lessonService.GetByIdAsync(lesson1.Id);
        var updatedLesson2 = await lessonService.GetByIdAsync(lesson2.Id);

        Assert.Equal(2, updatedLesson1!.Order);
        Assert.Equal(1, updatedLesson2!.Order);
    }
}
