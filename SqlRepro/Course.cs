using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlRepro;

[Table(nameof(Course))]
public class Course
{
    private Course()
    {

    }

    public Course(string title, Faculty faculty, Subject subject, Instructor instructor)
    {
        if(string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));

        Title = title;
        Faculty = faculty ?? throw new ArgumentNullException(nameof(faculty));
        Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        Instructor = instructor ?? throw new ArgumentNullException(nameof(instructor));
    }

    public Course(string title, Faculty faculty, Subject subject, Instructor instructor, int credit) : this(title, faculty, subject, instructor)
    {
        Credit = credit;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    [Range(1, 6)]
    public int Credit { get; set; } = 2;

    public int FacultyId { get; set; }

    public Faculty Faculty { get; set; } = null!;

    public int SubjectId { get; set; }

    public Subject Subject { get; set; } = null!;

    public int InstructorId { get; set; }

    public Instructor Instructor { get; set; } = null!;
}
