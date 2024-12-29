using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
{
    public async Task<IEnumerable<Department>> FindAllDepartmentsInfoAsync()
    {
        return await context.Departments
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Department>> FindAllUserDepartmentsAsync(Guid userId)
    {
        return await context.Departments
            .Where(d => d.Users.Any(u => u.Id == userId))
            .ToListAsync();
    }

    public async Task<Department?> FindDepartmentByIdAsync(int id)
    {
        return await context.Departments
            .Where(d => d.Id == id)
            .AsNoTracking()
            .Include(d => d.Subjects)
            .FirstOrDefaultAsync();
    }

    public async Task<Department?> FindUserDepartmentByIdAsync(Guid userId, int departmentId)
    {
        return await context.Departments
            .Include(d => d.Subjects) // Include related subjects
            .Where(d => d.Id == departmentId && d.Users.Any(u => u.Id == userId))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DepartmentExistsAsync(int id)
    {
        return await context.Departments.AnyAsync(d => d.Id == id);
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

    public async Task Save()
    {
        await context.SaveChangesAsync();
    }

    public async Task<Department?> FindDepartmentByNameAsync(string name)
    {
        return await context.Departments
            .Where(d => d.Name == name)
            .AsNoTracking()
            .Include(d => d.Subjects)
            .FirstOrDefaultAsync();
    }
}