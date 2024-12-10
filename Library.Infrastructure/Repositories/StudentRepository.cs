using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class StudentRepository(AppDbContext context) : IStudentRepository
{
    public async Task<IEnumerable<Student>> FindStudentsAsync()
    {
        return await context.Students
            .Include(s => s.User)
            .ToListAsync();
    }

    public async Task<Student?> FindStudentByIdAsync(Guid id)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.UserId == id)
            .Include(s => s.User)
            .FirstOrDefaultAsync();
    }

    public async Task<Student?> FindStudentByNameAsync(string name)
    {
        return await context.Students
            .AsNoTracking()
            .Include(s => s.User)
            .Where(s => s.User.UserName == name)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Student>> FindStudentsBySubjectAsync(Guid subjectId)
    {
        var studentsWithSubjectsAndUser = await context.Students
            .Where(s => s.Subjects.Any(sub => sub.Id == subjectId)) // Filter students by subject ID
            .Include(s => s.User)
            .ToListAsync();
        return studentsWithSubjectsAndUser;
    }

    public async Task DeleteStudentAsync(Student student)
    {
        context.Students.Remove(student);
        await Save();
    }

    private async Task Save()
    {
        await context.SaveChangesAsync();
    }
}