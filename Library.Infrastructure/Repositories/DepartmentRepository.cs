using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
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
            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            //TODO check department is not null
            if (department == null)
            {
                throw new KeyNotFoundException($"Can't found Department with ID = {id}");
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}