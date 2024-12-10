using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class LectureRepository(AppDbContext context) : ILectureRepository
{
    public async Task<IEnumerable<Lecture>> FindAllLecturesAsync()
    {
        return await context.Lectures
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Lecture?> FindLectureByIdAsync(Guid id)
    {
        return await context.Lectures
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Include(l => l.Subject)
            .FirstOrDefaultAsync();
    }

    public async Task<Lecture?> FindLectureByNameAsync(string name, Guid subjectId)
    {
        return await context.Lectures
            .AsNoTracking()
            .Where(l => l.Title == name && l.SubjectId == subjectId)
            .FirstOrDefaultAsync();
    }

    public async Task<string?> FindLectureFilePathByIdAsync(Guid id)
    {
        return await context.Lectures
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => l.FilePath)
            .FirstOrDefaultAsync();
    }

    public async Task AddLectureAsync(Lecture lecture)
    {
        await context.Lectures.AddAsync(lecture);
        await Save();
    }


    public async Task DeleteLectureAsync(Lecture lecture)
    {
        context.Lectures.Remove(lecture);
        await Save();
    }

    private async Task Save()
    {
        await context.SaveChangesAsync();
    }
}