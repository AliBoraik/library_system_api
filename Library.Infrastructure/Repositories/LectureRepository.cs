using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class LectureRepository(ApplicationDbContext context) : ILectureRepository
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
            .Include(l => l.Subject)
            .FirstOrDefaultAsync(l => l.LectureId == id);
    }

    public async Task<Lecture?> FindLectureByNameAsync(string name)
    {
        return await context.Lectures
            .AsNoTracking()
            .Include(l => l.Subject)
            .FirstOrDefaultAsync(l => l.Title == name);
    }

    public async Task<string?> FindLectureFilePathByIdAsync(Guid id)
    {
        return await context.Lectures
            .Where(l => l.LectureId == id)
            .Select(l => l.FilePath)
            .FirstOrDefaultAsync();
    }

    public async Task AddLectureAsync(Lecture lecture)
    {
        await context.Lectures.AddAsync(lecture);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLectureAsync(Lecture lecture)
    {
        context.Entry(lecture).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteLectureAsync(Lecture lecture)
    {
        context.Lectures.Remove(lecture);
        await context.SaveChangesAsync();
    }
}