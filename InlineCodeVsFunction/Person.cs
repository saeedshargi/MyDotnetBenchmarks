namespace InlineCodeVsFunction;

public class Person
{
    public int Id { get; }
    public string FullName { get; }
    public DateTime BirthDate { get; }

    public Person(int id, string fullName,DateTime birthDate)
    {
        Id = id;
        FullName = fullName;
        BirthDate = birthDate;
    }

    public override string ToString()
    {
        return $"Person with id: {Id} and fullName: {FullName} born on: {BirthDate}";
    }
}