namespace OnlineCoursePlatform.Application.DTOs;

public class CreateLessonDto
{
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public Guid CourseId { get; set; }
}

public class UpdateLessonDto
{
    public string Title { get; set; } = string.Empty;
}

public class ReorderLessonDto
{
    public int NewOrder { get; set; }
}
