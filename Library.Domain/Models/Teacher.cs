namespace Library.Domain.Models;

public class Teacher : User
{
    public ICollection<Book> Books { get; set; }
    public ICollection<Lecture> Lectures { get; set; }
}