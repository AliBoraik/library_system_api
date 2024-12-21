using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models;

public class Department
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Subject> Subjects { get; set; }
}