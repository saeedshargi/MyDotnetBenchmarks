using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FakeDataGeneration.DataGeneration;
using FakeDataGeneration.Dtos;
using FakeDataGeneration.Enums;
using FakeDataGeneration.Models;

namespace LoopsVsLinq;

[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[ExceptionDiagnoser]
public class Benchmarks
{
    private List<Student>? _students;

    [GlobalSetup]
    public async Task Setup()
    {
        var random = new Random(963258);
        var dataGenerator = new GenerateSudents(random);
        _students = (await dataGenerator.GenerateData(100)).ToList();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _students = null;
    }

    [Benchmark]
    public Task ForeachLoop()
    {
        if (_students == null) return Task.CompletedTask;
        var students = new List<StudentDto>();
        foreach (var student in _students)
        {
            students.Add(new StudentDto
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
        
        Console.WriteLine($"Students count is: {students.Count}");
        return Task.CompletedTask;
    }

    [Benchmark]
    public Task ForLoop()
    {
        if (_students == null) return Task.CompletedTask;
        var students = new List<StudentDto>();
        for (var index = 0; index < _students.Count; index++)
        {
            students.Add(new StudentDto
            {
                Id = _students[index].Id,
                FullName = $"{_students[index].FirstName} {_students[index].LastName}",
                EnrollmentData = _students[index].EnrollmentDate.ToString("d"),
                Enrollments = _students[index].Enrollments.Select(c => new EnrollmentDto
                {
                    CourseName = c.Course.Title,
                    Credits = c.Course.Credits,
                    Grade = Enum.GetName(typeof(Grade), c.Grade ?? Grade.NotGiven)
                })
            });
        }
        
        Console.WriteLine($"Students count is: {students.Count}");
        return Task.CompletedTask;
    }

    [Benchmark]
    public Task WhileLoop()
    {
        if (_students == null) return Task.CompletedTask;
        var students = new List<StudentDto>();
        var index = 0;
        while (index < _students.Count)
        {
            students.Add(new StudentDto
            {
                Id = _students[index].Id,
                FullName = $"{_students[index].FirstName} {_students[index].LastName}",
                EnrollmentData = _students[index].EnrollmentDate.ToString("d"),
                Enrollments = _students[index].Enrollments.Select(c => new EnrollmentDto
                {
                    CourseName = c.Course.Title,
                    Credits = c.Course.Credits,
                    Grade = Enum.GetName(typeof(Grade), c.Grade ?? Grade.NotGiven)
                })
            });
            index++;
        }
        
        Console.WriteLine($"Students count is: {students.Count}");
        return Task.CompletedTask;
    }

    [Benchmark]
    public Task LinqToList()
    {
        if (_students == null) return Task.CompletedTask;
        var students = _students.Select(student => new StudentDto
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
        
        Console.WriteLine($"Students count is: {students.Count}");
        return Task.CompletedTask;
    }
    
    [Benchmark]
    public Task LinqEnumerable()
    {
        if (_students == null) return Task.CompletedTask;
        var students = _students.Select(student => new StudentDto
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
        
        Console.WriteLine($"Students count is: {students.Count()}");
        return Task.CompletedTask;
    }
}