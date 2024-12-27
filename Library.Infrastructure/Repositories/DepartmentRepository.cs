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

    public async Task<Department?> FindUserDepartmentAsync(Guid userId)
    {
        var student = await context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId) // Find student by userId
            .Include(u => u.Department) // Include the department
            .ThenInclude(d => d.Subjects) // Include subjects in the department
            .FirstOrDefaultAsync();

        return student?.Department; // Return department with subjects or null if not found
    }

    public async Task<Department?> FindDepartmentByIdAsync(int id)
    {
        return await context.Departments
            .Where(d => d.Id == id)
            .AsNoTracking()
            .Include(d => d.Subjects)
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