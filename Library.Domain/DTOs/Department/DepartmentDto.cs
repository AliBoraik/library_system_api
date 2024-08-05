using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Department;

public class DepartmentDto : CreateDepartmentDto
{
    [Required]
    public Guid? DepartmentId { get; set; }
}