namespace ObjectCreation;

public class StudentInfo
{
    public int Id { get; private set; }

    public string FullName { get; private set; }

    public StudentInfo(int id, string firstName, string lastName)
    {
        Id = id;
        FullName = $"{firstName} {lastName}";
    }
}