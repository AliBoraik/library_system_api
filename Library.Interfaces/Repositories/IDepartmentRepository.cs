using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> FindAllDepartmentsInfoAsync();
    Task<Department?> FindDepartmentByIdAsync(Guid id);
    Task<bool> DepartmentExistsAsync(Guid id);
    Task AddDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(Department department);
}