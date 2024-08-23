using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsInfoAsync()
    {
        return await _context.Departments
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Department?> GetDepartmentByIdAsync(Guid id)
    {
        return await _context.Departments
            .AsNoTracking()
            .Include(d => d.Subjects)
            .FirstOrDefaultAsync(d => d.DepartmentId == id);
    }

    public async Task AddDepartmentAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await Save();
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        _context.Entry(department).State = EntityState.Modified;
        await Save();
    }

    public async Task DeleteDepartmentAsync(Department department)
    {
        _context.Departments.Remove(department);
        await Save();
    }

    private async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}