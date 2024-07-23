using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Subject;

public class CreateSubjectDto
{
    [Required] public string Name { get; init; } = null!;

    [Required] public string Description { get; init; } = null!;

    [Required] public Guid DepartmentId { get; init; }
}