using Library.Domain.DTOs.Department;

namespace Library.Interfaces.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDetailsDto> GetDepartmentByIdAsync(Guid id);
    Task<Guid> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    Task UpdateDepartmentAsync(DepartmentDto createDepartmentDto);
    Task DeleteDepartmentAsync(Guid id);
}