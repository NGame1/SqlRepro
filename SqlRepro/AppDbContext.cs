using Microsoft.EntityFrameworkCore;

namespace SqlRepro;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Faculty> Faculties => Set<Faculty>();

    public DbSet<Instructor> Instructors => Set<Instructor>();

    public DbSet<Subject> Subjects => Set<Subject>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
}
