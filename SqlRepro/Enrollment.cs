using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlRepro;

[Table(nameof(Enrollment))]
public class Enrollment
{
    private Enrollment()
    {

    }

    public Enrollment(Student student, Course course, double grade)
    {
        Student = student ?? throw new ArgumentNullException(nameof(student));
        Course = course ?? throw new ArgumentNullException(nameof(course));
        Grade = grade;
    }

    [Key]
    public int Id { get; set; }

    [Range(0.0, 20.0)]
    public double Grade { get; set; }

    public int StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;
}
