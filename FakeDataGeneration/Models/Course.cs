namespace FakeDataGeneration.Models;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
}