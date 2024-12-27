using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> FindAllDepartmentsInfoAsync();
    Task<Department?> FindUserDepartmentAsync(Guid userId);

    Task<Department?> FindDepartmentByIdAsync(int id);
    Task<bool> DepartmentExistsAsync(int id);
    Task AddDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(Department department);

    Task Save();
}