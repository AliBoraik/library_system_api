using Library.Domain.DTOs.Subject;

namespace Library.Domain.DTOs.Department;

public class DepartmentDetailsDto : DepartmentDto
{
    public ICollection<SubjectDto> Subjects { get; init; } = null!;
}