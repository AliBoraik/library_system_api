using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models;

public class Subject
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public ICollection<Lecture> Lectures { get; set; }
    public ICollection<Book> Books { get; set; }
    public List<Student> Students { get; } = [];

    public Guid TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
}