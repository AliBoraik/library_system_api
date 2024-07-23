namespace Library.Domain.DTOs.Department;

public class DepartmentInfoDto
{
    public Guid DepartmentId { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
}