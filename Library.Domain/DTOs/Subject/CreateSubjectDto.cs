using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Subject;

public class CreateSubjectDto
{
    [Required] public string Name { get; init; } = null!;

    [Required] public string Description { get; init; } = null!;

    [Required] public int DepartmentId { get; init; }
    [Required] public Guid TeacherId { get; init; }
}