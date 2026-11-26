
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlRepro;

void MockStudents(AppDbContext? dbContext)
{
    if (dbContext == null)
        throw new ArgumentNullException(nameof(dbContext));

    if (dbContext.Students.Any())
        return;

    AddStudent(dbContext, new Student("Ali Moradi", 20));
    AddStudent(dbContext, new Student("Babak Mohseni", 30));
    AddStudent(dbContext, new Student("Babak Khaleghi", 23));
    AddStudent(dbContext, new Student("Bita Mohammadi", 22));
    AddStudent(dbContext, new Student("Danial Faramarzi", 24));
    AddStudent(dbContext, new Student("Fereshte Mirzaei", 28));
    AddStudent(dbContext, new Student("Hoda Mohtashami", 18));
    AddStudent(dbContext, new Student("Masoud Feyzi", 21));
    AddStudent(dbContext, new Student("Nima Mousavi", 24));
    AddStudent(dbContext, new Student("Negin HasanZade", 25));
    AddStudent(dbContext, new Student("Ramin Rostami", 26));
    AddStudent(dbContext, new Student("Zahra Dehghan", 27));
    AddStudent(dbContext, new Student("Zahra Taheri", 27));
    AddStudent(dbContext, new Student("Ali Madani", 26));
    AddStudent(dbContext, new Student("Ali Mohammadi", 24));
    AddStudent(dbContext, new Student("Ali Roshanzad", 26));
    AddStudent(dbContext, new Student("Ali Tavakoli", 28));

    dbContext.SaveChanges();
}

void AddStudent(AppDbContext dbContext, Student student)
{
    dbContext.Students.Add(student);
}

void MockAcademicData(AppDbContext? dbContext)
{
    if (dbContext == null)
        throw new ArgumentNullException(nameof(dbContext));

    if (dbContext.Courses.Any())
        return;

    var computerFaculty = new Faculty("Computer Engineering");
    var electricalFaculty = new Faculty("Electrical Engineering");

    var mathematics = new Subject("Mathematics");
    var programming = new Subject("Programming");
    var databases = new Subject("Databases");

    var ahmadi = new Instructor("Reza Ahmadi");
    var nazemi = new Instructor("Sara Nazemi");

    dbContext.Faculties.Add(computerFaculty);
    dbContext.Faculties.Add(electricalFaculty);

    dbContext.Subjects.Add(mathematics);
    dbContext.Subjects.Add(programming);
    dbContext.Subjects.Add(databases);

    dbContext.Instructors.Add(ahmadi);
    dbContext.Instructors.Add(nazemi);

    dbContext.Courses.Add(new Course("Algorithms", computerFaculty, programming, ahmadi, 3));
    dbContext.Courses.Add(new Course("Databases 1", computerFaculty, databases, nazemi, 3));
    dbContext.Courses.Add(new Course("Calculus 1", computerFaculty, mathematics, ahmadi, 2));
    dbContext.Courses.Add(new Course("Digital Circuits", electricalFaculty, mathematics, nazemi, 3));

    dbContext.SaveChanges();
}

void MockEnrollments(AppDbContext? dbContext)
{
    if (dbContext == null)
        throw new ArgumentNullException(nameof(dbContext));

    if (dbContext.Enrollments.Any())
        return;

    var random = new Random(2024);
    var students = dbContext.Students.ToArray();
    var courses = dbContext.Courses.ToArray();

    foreach (var student in students)
    {
        foreach (var course in courses)
        {
            if (random.Next(100) < 60)
                dbContext.Enrollments.Add(new Enrollment(student, course, random.Next(10, 21)));
        }
    }

    dbContext.SaveChanges();
}

var connectionString = "Data Source=bootcamp.db";

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .Options;

var db = new AppDbContext(options);

MockStudents(db);

MockAcademicData(db);

MockEnrollments(db);

var q = from s in db.Students
        group s by s.FirstName into g
        orderby g.Key descending
        select new
        {
            FirstName = g.Key,
            MinAge = g.Min(s => s.Age),
            MaxAge = g.Max(s => s.Age),
            Average = g.Average(s => s.Age),
            Count = g.Count()
        };
var lst = q.ToList();

// Ordering by an aggregate (Count) is not translated to SQL by the SQLite provider,
// so it is applied in memory after the query is materialized
foreach (var item in lst.OrderBy(i => i.Count).ThenByDescending(i => i.FirstName))
{
    Console.WriteLine($"First Name: {item.FirstName}, Min Age: {item.MinAge}, Max Age: {item.MaxAge}, Average Age: {item.Average}, Count: {item.Count}");
}

// Courses with their instructor, faculty and subject (EF joins all navigations in one SQL)
var courseCatalog = db.Courses
    .Select(c => new
    {
        c.Title,
        Instructor = c.Instructor.FullName,
        Faculty = c.Faculty.Name,
        Subject = c.Subject.Title
    })
    .OrderBy(c => c.Title);

foreach (var course in courseCatalog)
    Console.WriteLine($"{course.Title} -> {course.Instructor} ({course.Faculty} / {course.Subject})");