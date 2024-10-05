using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models;

public class Student
{
    [Key] public Guid Id { get; set; }

    public virtual User User { get; set; }
}