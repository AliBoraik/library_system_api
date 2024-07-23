using Library.Domain.DTOs;
using Library.Domain.DTOs.Department;

namespace Library.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentInfoDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(Guid id);
        Task AddDepartmentAsync(DepartmentDto departmentDto);
        Task UpdateDepartmentAsync(DepartmentDto departmentDto);
        Task DeleteDepartmentAsync(Guid id);
    }
}