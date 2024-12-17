namespace Library.Domain.Models;

public class Subject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; }
    public ICollection<Lecture> Lectures { get; set; }
    public ICollection<Book> Books { get; set; }
    public List<Student> Students { get; } = [];

    public Guid TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
}