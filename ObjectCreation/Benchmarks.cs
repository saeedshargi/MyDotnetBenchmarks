using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FakeDataGeneration.DataGeneration;
using FakeDataGeneration.Models;

namespace ObjectCreation;

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
    public Task WithStudentInfo()
    {
        if (_students == null) return Task.CompletedTask;
        var students = _students.Select(x => new StudentInfo(x.Id,x.FirstName,x.LastName));
        return Task.CompletedTask;
    }
    
    [Benchmark]
    public Task WithStudentInfoRecord()
    {
        if (_students == null) return Task.CompletedTask;
        var students = _students.Select(x => new StudentInfoRecord(x.Id,$"{x.FirstName} {x.LastName}"));
        return Task.CompletedTask;
    }
    
    [Benchmark]
    public Task WithStudentInfoFactory()
    {
        if (_students == null) return Task.CompletedTask;
        var students = _students.Select(x => StudentInfoFactory.CreateStudentInfo(x.Id,x.FirstName,x.LastName));
        return Task.CompletedTask;
    }
}