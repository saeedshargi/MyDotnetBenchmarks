using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FakeDataGeneration.DataGeneration;
using FakeDataGeneration.Dtos;
using FakeDataGeneration.Enums;
using FakeDataGeneration.Models;

namespace Collections;

[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[ExceptionDiagnoser]
public class Benchmarks
{
    private IQueryable<Student>? _students;
    
    [GlobalSetup]
    public async Task Setup()
    {
        var random = new Random(753159);
        var dataGenerator = new GenerateSudents(random);
        _students = (await dataGenerator.GenerateData(100)).AsQueryable();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _students = null;
    }

    [Benchmark]
    public Task GetAsEnumerable()
    {
        if (_students is null) return Task.CompletedTask;

        var students = GetStudents(_students.AsEnumerable());
        
        Console.WriteLine($"Students count is: {students.Count()}");

        return Task.CompletedTask;
    }
    
    [Benchmark]
    public Task GetAsList()
    {
        if (_students is null) return Task.CompletedTask;

        var students = GetStudents(_students.ToList());
        
        Console.WriteLine($"Students count is: {students.Count}");

        return Task.CompletedTask;
    }
    
    [Benchmark]
    public Task GetAsArray()
    {
        if (_students is null) return Task.CompletedTask;

        var students = GetStudents(_students.ToArray());
        
        Console.WriteLine($"Students count is: {students.Length}");

        return Task.CompletedTask;
    }
    
    [Benchmark]
    public Task GetAsReadOnlyCollection()
    {
        if (_students is null) return Task.CompletedTask;

        var students = GetStudents(_students.ToList().AsReadOnly());
        
        Console.WriteLine($"Students count is: {students.Count}");

        return Task.CompletedTask;
    }

    private IEnumerable<StudentDto> GetStudents(IEnumerable<Student> students)
    {
        return students.Select(student => new StudentDto
        {
            Id = student.Id,
            FullName = $"{student.FirstName} {student.LastName}",
            EnrollmentData = student.EnrollmentDate.ToString("d"),
            Enrollments = student.Enrollments.Select(c => new EnrollmentDto
            {
                CourseName = c.Course.Title,
                Credits = c.Course.Credits,
                Grade = Enum.GetName(typeof(Grade), c.Grade ?? Grade.NotGiven)
            })
        });
    }
    
    private List<StudentDto> GetStudents(List<Student> students)
    {
        return students.Select(student => new StudentDto
        {
            Id = student.Id,
            FullName = $"{student.FirstName} {student.LastName}",
            EnrollmentData = student.EnrollmentDate.ToString("d"),
            Enrollments = student.Enrollments.Select(c => new EnrollmentDto
            {
                CourseName = c.Course.Title,
                Credits = c.Course.Credits,
                Grade = Enum.GetName(typeof(Grade), c.Grade ?? Grade.NotGiven)
            })
        }).ToList();
    }
    
    private StudentDto[] GetStudents(Student[] students)
    {
        return students.Select(student => new StudentDto
        {
            Id = student.Id,
            FullName = $"{student.FirstName} {student.LastName}",
            EnrollmentData = student.EnrollmentDate.ToString("d"),
            Enrollments = student.Enrollments.Select(c => new EnrollmentDto
            {
                CourseName = c.Course.Title,
                Credits = c.Course.Credits,
                Grade = Enum.GetName(typeof(Grade), c.Grade ?? Grade.NotGiven)
            })
        }).ToArray();
    }
    
    private IReadOnlyCollection<StudentDto> GetStudents(IReadOnlyCollection<Student> students)
    {
        return students.Select(student => new StudentDto
        {
            Id = student.Id,
            FullName = $"{student.FirstName} {student.LastName}",
            EnrollmentData = student.EnrollmentDate.ToString("d"),
            Enrollments = student.Enrollments.Select(c => new EnrollmentDto
            {
                CourseName = c.Course.Title,
                Credits = c.Course.Credits,
                Grade = Enum.GetName(typeof(Grade), c.Grade ?? Grade.NotGiven)
            })
        }).ToList().AsReadOnly();
    }
    
}