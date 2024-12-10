using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models;

public class Student
{
    [Key]
    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public List<Subject> Subjects { get; } = [];
}