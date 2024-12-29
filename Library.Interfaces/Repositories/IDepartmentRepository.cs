using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> FindAllDepartmentsInfoAsync();
    Task<IEnumerable<Department>> FindAllUserDepartmentsAsync(Guid userId);

    Task<Department?> FindDepartmentByIdAsync(int departmentId);
    Task<Department?> FindUserDepartmentByIdAsync(Guid userId , int departmentId);
    Task<bool> DepartmentExistsAsync(int departmentId);
    Task AddDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(Department department);

    Task Save();
}