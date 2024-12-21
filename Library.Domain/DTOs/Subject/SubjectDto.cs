using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Subject;

public class SubjectDto
{
    [Required] public int Id { get; init; }
    [Required] public string Name { get; init; } = null!;

    [Required] public string Description { get; init; } = null!;

    [Required] public int DepartmentId { get; init; }
}