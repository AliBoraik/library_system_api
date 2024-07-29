using Library.Application.Exceptions;
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
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        var departmentExists = await _context.Departments.AnyAsync(d => d.DepartmentId == department.DepartmentId);
        if (!departmentExists)
            throw new NotFoundException($"Can't found Department with ID = {department.DepartmentId}");
        _context.Entry(department).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDepartmentAsync(Guid id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null) throw new NotFoundException($"Can't found Department with ID = {id}");
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }
}