using Microsoft.EntityFrameworkCore;
using OnlineCoursePlatform.Application.Exceptions;
using OnlineCoursePlatform.Application.Services;
using OnlineCoursePlatform.Domain.Entities;
using OnlineCoursePlatform.Domain.Enums;
using OnlineCoursePlatform.Domain.Interfaces;
using OnlineCoursePlatform.Infrastructure.Data;
using OnlineCoursePlatform.Infrastructure.Repositories;

namespace OnlineCoursePlatform.Tests;

public class CourseServiceTests
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
    public async Task PublishCourse_WithLessons_ShouldSucceed()
    {
        // Arrange
        var uow = GetUnitOfWork();
        var service = new CourseService(uow);

        var course = await service.CreateAsync("Test Course");
        
        var lessonService = new LessonService(uow);
        await lessonService.CreateAsync(course.Id, "Lesson 1", 1);

        // Act
        var result = await service.PublishAsync(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(CourseStatus.Published, result.Status);
    }

    [Fact]
    public async Task PublishCourse_WithoutLessons_ShouldFail()
    {
        // Arrange
        var uow = GetUnitOfWork();
        var service = new CourseService(uow);

        var course = await service.CreateAsync("Test Course Without Lessons");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(
            async () => await service.PublishAsync(course.Id));

        Assert.Contains("without active lessons", exception.Message);
    }

    [Fact]
    public async Task DeleteCourse_ShouldBeSoftDelete()
    {
        // Arrange
        var uow = GetUnitOfWork();
        var service = new CourseService(uow);

        var course = await service.CreateAsync("Course to Delete");
        var courseId = course.Id;

        // Act
        await service.DeleteAsync(courseId);

        // Assert
        var deletedCourse = await service.GetByIdAsync(courseId, includeDeleted: false);
        Assert.Null(deletedCourse);

        var courseWithDeleted = await service.GetByIdAsync(courseId, includeDeleted: true);
        Assert.NotNull(courseWithDeleted);
        Assert.True(courseWithDeleted.IsDeleted);
    }
}
