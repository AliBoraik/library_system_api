using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class SubjectRepository(ApplicationDbContext context) : ISubjectRepository
{
    public async Task<IEnumerable<Subject>> FindAllSubjectsInfoAsync()
    {
        return await context.Subjects
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Subject?> FindSubjectByIdAsync(Guid id)
    {
        return await context.Subjects
            .AsNoTracking()
            .Include(s => s.Lectures)
            .Include(s => s.Books)
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.SubjectId == id);
    }

    public async Task<bool> SubjectExistsAsync(Guid id)
    {
        return await context.Subjects.AnyAsync(d => d.SubjectId == id);
    }

    public async Task AddSubjectAsync(Subject subject)
    {
        await context.Subjects.AddAsync(subject);
        await context.SaveChangesAsync();
    }

    public async Task UpdateSubjectAsync(Subject subject)
    {
        context.Entry(subject).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteSubjectAsync(Subject subject)
    {
        context.Subjects.Remove(subject);
        await context.SaveChangesAsync();
    }
}