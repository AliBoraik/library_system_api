using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models;

public class Teacher
{
    [Key] public Guid TeacherId { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public ICollection<Subject> Subjects { get; set; }
}