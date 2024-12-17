using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class SubjectRepository(AppDbContext context) : ISubjectRepository
{
    public async Task<IEnumerable<Subject>> FindAllSubjectsInfoAsync()
    {
        return await context.Subjects
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> FindStudentIdsBySubjectAsync(Guid subjectId)
    {
        var studentIds = await context.Subjects
            .Where(s => s.Id == subjectId)
            .SelectMany(s => s.Students) // Flatten the collection of Students
            .Select(student => student.Id) // Select only the Student ID
            .ToListAsync();
        return studentIds;
    }

    public async Task<Subject?> FindSubjectDetailsByIdAsync(Guid id)
    {
        return await context.Subjects
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Include(s => s.Lectures)
            .Include(s => s.Books)
            .AsSplitQuery()
            .FirstOrDefaultAsync();
    }

    public async Task<Subject?> FindSubjectByIdAsync(Guid id)
    {
        return await context.Subjects
            .AsNoTracking()
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task AddStudentToSubjectAsync(Guid studentId, Guid subjectId)
    {
        var student = await context.Students.FindAsync(studentId);
        var subject = await context.Subjects.FindAsync(subjectId);

        if (student != null && subject != null)
        {
            // Ensure the Students collection is initialized
            subject.Students.Add(student);
            await Save();
        }
    }

    public async Task<bool> SubjectExistsAsync(Guid id)
    {
        return await context.Subjects.AnyAsync(d => d.Id == id);
    }

    public async Task AddSubjectAsync(Subject subject)
    {
        await context.Subjects.AddAsync(subject);
        await Save();
    }

    public async Task UpdateSubjectAsync(Subject subject)
    {
        context.Entry(subject).State = EntityState.Modified;
        await Save();
    }

    public async Task DeleteSubjectAsync(Subject subject)
    {
        context.Subjects.Remove(subject);
        await Save();
    }

    private async Task Save()
    {
        await context.SaveChangesAsync();
    }
}