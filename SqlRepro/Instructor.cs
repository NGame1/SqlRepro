using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlRepro;

[Table(nameof(Instructor))]
public class Instructor
{
    private Instructor()
    {

    }

    public Instructor(string fullName)
    {
        if(string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentNullException(nameof(fullName));

        FullName = fullName;
    }

    public Instructor(string fullName, string email) : this(fullName)
    {
        Email = email;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public List<Course> Courses { get; } = new();
}
