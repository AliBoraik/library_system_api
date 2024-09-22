namespace Library.Domain.Models;

public class Teacher : User
{
    public ICollection<Subject> Subjects { get; set; }
}