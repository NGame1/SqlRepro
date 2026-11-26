using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlRepro;

[Table(nameof(Student))]
public class Student
{
    private Student()
    {

    }

    public Student(string fullName)
    {
        if(string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentNullException(nameof(fullName));

        var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (nameParts.Length < 2)
            throw new ArgumentException("Full name must include both first and last name.", nameof(fullName));

        FirstName = nameParts.First();
        LastName = nameParts.Last();
    }

    public Student(string fullName, int age) : this(fullName)
    {
        Age = age;
    }

    public Student(string fullName, string email) : this(fullName)
    {
        Email = email;
    }

    public Student(string fullName, string email, int age) : this(fullName)
    {
        Age = age;
        Email = email;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Range(18, 80)]
    public int Age { get; set; } = 0;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
