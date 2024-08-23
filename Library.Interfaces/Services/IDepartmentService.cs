using Library.Domain;
using Library.Domain.DTOs.Department;

namespace Library.Interfaces.Services;

public interface IDepartmentService
{
    Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllDepartmentsAsync();
    Task<Result<DepartmentDetailsDto, Error>> GetDepartmentByIdAsync(Guid id);
    Task<Result<Guid, Error>> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    Task<Result<Ok, Error>> UpdateDepartmentAsync(DepartmentDto createDepartmentDto);
    Task<Result<Ok, Error>> DeleteDepartmentAsync(Guid id);
}