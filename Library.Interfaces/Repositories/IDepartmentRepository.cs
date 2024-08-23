using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllDepartmentsInfoAsync();
    Task<Department?> GetDepartmentByIdAsync(Guid id);
    Task AddDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(Department department);
}