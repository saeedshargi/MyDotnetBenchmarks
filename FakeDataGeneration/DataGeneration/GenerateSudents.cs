using Bogus;
using FakeDataGeneration.Enums;
using FakeDataGeneration.Models;

namespace FakeDataGeneration.DataGeneration;

public class GenerateSudents
{
    public GenerateSudents(Random random)
    {
        Randomizer.Seed = random;
    }

    public Task<IEnumerable<Student>> GenerateData(int count)
    {
        var studentId = 1;
        var courseId = 1;
        var enrollmentId = 1;

        var courses = new[] { "Programming", "Algorithm", "Database", "OS" };

        var studentFaker = new Faker<Student>()
            .RuleFor(f => f.Id, _ => studentId++)
            .RuleFor(f => f.FirstName, f => f.Person.FirstName)
            .RuleFor(f => f.LastName, f => f.Person.LastName)
            .RuleFor(f => f.EnrollmentDate, f => f.Date.Past(1));

        var courseFaker = new Faker<Course>()
            .RuleFor(f => f.CourseId, _ => courseId++)
            .RuleFor(f => f.Title, f => f.PickRandom(courses))
            .RuleFor(f => f.Credits, f => f.Random.Number(100, 500));
        
        var enrollmentFaker = new Faker<Enrollment>()
            .RuleFor(f => f.EnrollmentId, f => enrollmentId++)
            .RuleFor(f => f.CourseId, courseId++)
            .RuleFor(f => f.Grade, f => f.PickRandom<Grade>())
            .RuleFor(f => f.Course, f => courseFaker);

        studentFaker.RuleFor(f => f.Enrollments, f => enrollmentFaker.Generate(3).ToList());

        return Task.FromResult<IEnumerable<Student>>(studentFaker.Generate(count));
    }
}