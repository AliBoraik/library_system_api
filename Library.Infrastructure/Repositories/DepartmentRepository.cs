using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class DepartmentRepository(ApplicationDbContext context) : IDepartmentRepository
{
    public async Task<IEnumerable<Department>> FindAllDepartmentsInfoAsync()
    {
        return await context.Departments
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Department?> FindDepartmentByIdAsync(Guid id)
    {
        return await context.Departments
            .Where(d => d.DepartmentId == id)
            .AsNoTracking()
            .Include(d => d.Subjects)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DepartmentExistsAsync(Guid id)
    {
        return await context.Departments.AnyAsync(d => d.DepartmentId == id);
    }

    public async Task AddDepartmentAsync(Department department)
    {
        await context.Departments.AddAsync(department);
        await Save();
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        context.Entry(department).State = EntityState.Modified;
        await Save();
    }

    public async Task DeleteDepartmentAsync(Department department)
    {
        context.Departments.Remove(department);
        await Save();
    }

    public async Task<Department?> FindDepartmentByNameAsync(string name)
    {
        return await context.Departments
            .Where(d => d.Name == name)
            .AsNoTracking()
            .Include(d => d.Subjects)
            .FirstOrDefaultAsync();
    }

    private async Task Save()
    {
        await context.SaveChangesAsync();
    }
}