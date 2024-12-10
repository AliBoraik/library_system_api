namespace Library.Domain.Models;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Subject> Subjects { get; set; }
}