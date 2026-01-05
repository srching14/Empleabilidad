using Microsoft.EntityFrameworkCore;
using OnlineCoursePlatform.Domain.Entities;
using OnlineCoursePlatform.Domain.Enums;
using OnlineCoursePlatform.Domain.Interfaces;
using OnlineCoursePlatform.Infrastructure.Data;

namespace OnlineCoursePlatform.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
        var query = includeDeleted 
            ? _context.Courses.IgnoreQueryFilters()
            : _context.Courses;
            
        return await query
            .Include(c => c.Lessons.Where(l => !l.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(IEnumerable<Course> Courses, int TotalCount)> SearchAsync(
        string? searchQuery, 
        CourseStatus? status, 
        int page, 
        int pageSize)
    {
        var query = _context.Courses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(c => c.Title.Contains(searchQuery));
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        var totalCount = await query.CountAsync();

        var courses = await query
            .Include(c => c.Lessons.Where(l => !l.IsDeleted))
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (courses, totalCount);
    }

    public void Add(Course course)
    {
        _context.Courses.Add(course);
    }

    public void Update(Course course)
    {
        _context.Courses.Update(course);
    }
}
