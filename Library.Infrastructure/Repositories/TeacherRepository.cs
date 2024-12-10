using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class TeacherRepository(AppDbContext context) : ITeacherRepository
{
    public async Task<IEnumerable<Teacher>> FindTeachersAsync()
    {
        return await context.Teachers
            .AsNoTracking()
            .Include(t => t.User)
            .ToListAsync();
    }

    public async Task<Teacher?> FindTeacherByIdAsync(Guid id)
    {
        return await context.Teachers
            .AsNoTracking()
            .Where(s => s.UserId == id)
            .Include(t => t.User)
            .FirstOrDefaultAsync();
    }

    public async Task<Teacher?> FindTeacherByNameAsync(string name)
    {
        return await context.Teachers
            .AsNoTracking()
            .Include(s => s.User)
            .Where(t => t.User.UserName == name)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task DeleteTeacherAsync(Teacher teacher)
    {
        context.Teachers.Remove(teacher);
        await Save();
    }

    private async Task Save()
    {
        await context.SaveChangesAsync();
    }
}