using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace InlineCodeVsFunction;

[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[ExceptionDiagnoser]
public class PersonBenchmark
{
    private readonly List<Person> _persons = new();

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < 10000; i++)
        {
            _persons.Add(new Person(i,$"Test Person {i}",DateTime.Now));
        }
    }

    [Benchmark]
    public void InlineCode()
    {
        foreach (var person in _persons)
        {
            Console.WriteLine(person.ToString());
        }
    }
    
    [Benchmark]
    public void ExtractMethod()
    {
        foreach (var person in _persons)
        {
            Print(person);
        }
    }

    private void Print(Person person)
    {
        Console.WriteLine(person.ToString());
    }
}