using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Department;

public class CreateDepartmentDto
{
    [Required] public string Name { get; init; } = null!;

    [Required] public string Description { get; init; } = null!;
}