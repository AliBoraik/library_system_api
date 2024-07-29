using Library.Application.Exceptions;
using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly ApplicationDbContext _context;

    public SubjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Subject>> GetAllSubjectsInfoAsync()
    {
        return await _context.Subjects
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Subject?> GetSubjectByIdAsync(Guid id)
    {
        return await _context.Subjects
            .AsNoTracking()
            .Include(s => s.Lectures)
            .Include(s => s.Books)
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.SubjectId == id);
    }

    public async Task AddSubjectAsync(Subject subject)
    {
        var departmentsExists = await _context.Departments.AnyAsync(d => d.DepartmentId == subject.DepartmentId);
        if (!departmentsExists)
            throw new NotFoundException($"Not found department with id = {subject.DepartmentId}");
        await _context.Subjects.AddAsync(subject);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubjectAsync(Subject subject)
    {
        var subjectExists = await _context.Subjects.AnyAsync(d => d.SubjectId == subject.SubjectId);
        if (!subjectExists) throw new NotFoundException($"Can't found subject with ID = {subject.SubjectId}");
        var departmentsExists = await _context.Departments.AnyAsync(d => d.DepartmentId == subject.DepartmentId);
        if (!departmentsExists)
            throw new NotFoundException($"Not found department with id = {subject.DepartmentId}");
        _context.Entry(subject).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSubjectAsync(Guid id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) throw new NotFoundException($"Can't found subject with ID = {id}");
        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();
    }
}