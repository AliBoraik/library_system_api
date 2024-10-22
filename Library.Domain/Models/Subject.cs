namespace Library.Domain.Models;

public class Subject
{
    public Guid SubjectId { get; set; }
    public Guid DepartmentId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Department Department { get; set; }
    public ICollection<Lecture> Lectures { get; set; }
    public ICollection<Book> Books { get; set; }

    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
}