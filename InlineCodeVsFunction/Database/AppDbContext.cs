using Microsoft.EntityFrameworkCore;

namespace InlineCodeVsFunction.Database;

public class AppDbContext: DbContext
{
    private const string DatabaseNameAndPath = @"\Database\Persons.db";
    public DbSet<Person> Persons { get; set; }

    private string DbPath { get; }

    public AppDbContext()
    {
        var directory = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar);
        var slice = new ArraySegment<string>(directory, 0, directory.Length - 4);
        var path = Path.Combine(slice.ToArray());
        DbPath = Path.Join(path, DatabaseNameAndPath);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var directory = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar);
        var slice = new ArraySegment<string>(directory, 0, directory.Length - 8);
        var path = Path.Combine(slice.ToArray());
        optionsBuilder.UseSqlite(File.Exists(DbPath) || string.IsNullOrEmpty(path) ? $"Data Source={DbPath}" : $"Data Source={path}{DatabaseNameAndPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(p =>
        {
            p.ToTable("Persons");
            p.HasKey(c => c.Id);
            p.Property(c => c.Id).IsRequired();
            p.Property(c => c.FullName).HasMaxLength(100);
            p.Property(c => c.BirthDate);
        });
        base.OnModelCreating(modelBuilder);
    }
}