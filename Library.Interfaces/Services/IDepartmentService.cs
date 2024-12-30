using Library.Domain.DTOs.Department;
using Library.Domain.Results;
using Library.Domain.Results.Common;

namespace Library.Interfaces.Services;

public interface IDepartmentService
{
    Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllDepartmentsAsync();
    Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllUserDepartmentsAsync(Guid userId);
    Task<Result<DepartmentDetailsDto, Error>> GetDepartmentByIdAsync(int departmentId);
    Task<Result<DepartmentDetailsDto, Error>> GetUserDepartmentByIdAsync(Guid userId, int departmentId);
    Task<Result<int, Error>> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    Task<Result<Ok, Error>> UpdateDepartmentAsync(DepartmentDto createDepartmentDto);
    Task<Result<Ok, Error>> DeleteDepartmentAsync(int id);
}