using OnlineCoursePlatform.Domain.Interfaces;
using OnlineCoursePlatform.Infrastructure.Data;

namespace OnlineCoursePlatform.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ICourseRepository? _courses;
    private ILessonRepository? _lessons;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICourseRepository Courses => _courses ??= new CourseRepository(_context);
    public ILessonRepository Lessons => _lessons ??= new LessonRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
