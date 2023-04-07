using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using InlineCodeVsFunction.Database;
using Microsoft.EntityFrameworkCore;

namespace InlineCodeVsFunction;
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[ExceptionDiagnoser]
public class PersonWithDatabaseBenchmark
{
    private readonly List<Person> _persons = new();

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < 200; i++)
        {
            _persons.Add(new Person(i, $"Test Person {i}", DateTime.Now));
        }
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await using var db = new AppDbContext();
        await db.Persons.ExecuteDeleteAsync();
        await db.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertInlineCode()
    {
        var addedPerson = _persons.ElementAt(50);
        await using var db = new AppDbContext();
        await db.Persons.AddAsync(addedPerson);
        await db.SaveChangesAsync();
        
        await db.Persons.ExecuteDeleteAsync();
        await db.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertExtractMethod()
    {
        var addedPersons = _persons.ElementAt(50);
        await Add(addedPersons);
    }
    
    [Benchmark]
    public async Task UpdateInlineCode()
    {
        var addedPerson = _persons.ElementAt(50);
        await using var db = new AppDbContext();
        await db.Persons.AddAsync(addedPerson);
        await db.SaveChangesAsync();
        
        var existPerson = await db.Persons.FirstOrDefaultAsync(c => c.Id == addedPerson.Id);
        if (existPerson is null)
        {
            return;
        }
        existPerson.Update("Test edit",DateTime.Now);
        db.Persons.Update(existPerson);
        await db.SaveChangesAsync();
        
        await db.Persons.ExecuteDeleteAsync();
        await db.SaveChangesAsync();
    }

    [Benchmark]
    public async Task UpdateExtractMethod()
    {
        var person = _persons.ElementAt(50);
        await Edit(person);
    }
    
    [Benchmark]
    public async Task DeleteInlineCode()
    {
        var addedPerson = _persons.ElementAt(50);
        await using var db = new AppDbContext();
        await db.Persons.AddAsync(addedPerson);
        await db.SaveChangesAsync();
        
        db.Persons.Remove(addedPerson);
        await db.SaveChangesAsync();
    }

    [Benchmark]
    public async Task DeleteExtractMethod()
    {
        var person = _persons.ElementAt(50);
        await Delete(person);
    }
    
    private async Task Add(Person person)
    {
        await using var db = new AppDbContext();
        await db.Persons.AddAsync(person);
        await db.SaveChangesAsync();
        
        await ClearPersons();
    }
    
    private async Task Edit(Person person)
    {
        await using var db = new AppDbContext();
        
        await db.Persons.AddAsync(person);
        await db.SaveChangesAsync();
        
        var existPerson = await db.Persons.FirstOrDefaultAsync(c => c.Id == person.Id);
        if (existPerson is null)
        {
            return;
        }
        existPerson.Update("Test edit",DateTime.Now);
        db.Persons.Update(existPerson);
        await db.SaveChangesAsync();

        await ClearPersons();
    }

    private async Task Delete(Person person)
    {
        await using var db = new AppDbContext();
        await db.Persons.AddAsync(person);
        await db.SaveChangesAsync();
        
        db.Persons.Remove(person);
        await db.SaveChangesAsync();
    }

    private async Task ClearPersons()
    {
        await using var db = new AppDbContext();
        await db.Persons.ExecuteDeleteAsync();
        await db.SaveChangesAsync();
    }
}