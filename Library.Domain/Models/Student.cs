using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain.Models;

public class Student 
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Id))]
    public virtual User User { get; set; }
    

    
}