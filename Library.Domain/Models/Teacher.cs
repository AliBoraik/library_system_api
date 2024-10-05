using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models;

public class Teacher
{
    [Key] public Guid Id { get; set; }

    public virtual User User { get; set; }
    public ICollection<Subject> Subjects { get; set; }
}