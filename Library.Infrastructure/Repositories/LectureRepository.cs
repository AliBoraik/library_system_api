using Library.Application.Exceptions;
using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class LectureRepository : ILectureRepository
{
    private readonly ApplicationDbContext _context;

    public LectureRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Lecture>> GetAllLecturesAsync()
    {
        return await _context.Lectures
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Lecture?> GetLectureByIdAsync(Guid id)
    {
        return await _context.Lectures
            .AsNoTracking()
            .Include(l => l.Subject)
            .FirstOrDefaultAsync(l => l.LectureId == id);
    }

    public async Task<string> GetLectureFilePathByIdAsync(Guid id)
    {
        var filePath = await _context.Lectures
            .Where(l => l.LectureId == id)
            .Select(l => l.FilePath)
            .FirstOrDefaultAsync();
        Console.WriteLine();
        if (filePath == null)
            throw new NotFoundException($"Can't found Lecture with ID = {id}");
        return filePath;
    }

    public async Task AddLectureAsync(Lecture lecture)
    {
        await _context.Lectures.AddAsync(lecture);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLectureAsync(Lecture lecture)
    {
        var lectureExists = await _context.Lectures.FindAsync(lecture.LectureId);
        if (lectureExists == null)
            throw new NotFoundException($"Can't found Lecture with ID = {lecture.LectureId}");
        _context.Entry(lecture).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLectureAsync(Guid id)
    {
        var lecture = await _context.Lectures.FindAsync(id);
        if (lecture == null) throw new NotFoundException($"Can't found Lecture with ID = {id}");
        _context.Lectures.Remove(lecture);
        await _context.SaveChangesAsync();
    }
}