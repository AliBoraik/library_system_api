using Library.Domain.DTOs;

namespace Library.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentInfoDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(int id);
        Task AddDepartmentAsync(DepartmentDto departmentDto);
        Task UpdateDepartmentAsync(DepartmentDto departmentDto);
        Task DeleteDepartmentAsync(int id);
    }
}