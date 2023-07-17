namespace ObjectCreation;

public class StudentInfoFactory
{
    public int Id { get; private set; }

    public string FullName { get; private set; }

    private StudentInfoFactory(int id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }

    public static StudentInfoFactory CreateStudentInfo(int id, string name,string lastName)
    {
        return new StudentInfoFactory(id, $"{name} {lastName}");
    }
}