using System.Text.Json.Serialization;
using Library.Domain.DTOs.Subject;

namespace Library.Domain.DTOs.Department;

public class DepartmentDetailsDto : DepartmentDto
{
    [JsonPropertyOrder(2)]
    public ICollection<SubjectDto> Subjects { get; init; } = null!;
}