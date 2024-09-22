using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Department;

public class DepartmentDto
{
    [Required] public Guid? DepartmentId { get; set; }
    [Required] public string Name { get; init; } = null!;
    [Required] public string Description { get; init; } = null!;
}