namespace FakeDataGeneration.Dtos;

public class StudentDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EnrollmentData { get; set; } = string.Empty;
    public IEnumerable<EnrollmentDto> Enrollments { get; set; }
}