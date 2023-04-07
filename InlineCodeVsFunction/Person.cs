namespace InlineCodeVsFunction;

public class Person
{
    public int Id { get; private set; }
    public string FullName { get; private set; }
    public DateTime BirthDate { get; private set; }

    public Person(int id, string fullName,DateTime birthDate)
    {
        Id = id;
        FullName = fullName;
        BirthDate = birthDate;
    }

    public Person()
    {
        
    }

    public void Update(string fullName, DateTime birthDate)
    {
        FullName = fullName;
        BirthDate = birthDate;
    }

    public override string ToString()
    {
        return $"Person with id: {Id} and fullName: {FullName} born on: {BirthDate}";
    }
}