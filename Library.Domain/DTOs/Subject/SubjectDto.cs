using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Subject;

public class SubjectDto
{
    [Required] public Guid Id { get; init; }
    [Required] public string Name { get; init; } = null!;

    [Required] public string Description { get; init; } = null!;

    [Required] public Guid DepartmentId { get; init; }
}