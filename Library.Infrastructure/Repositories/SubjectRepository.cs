using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects
                .AsNoTracking()
                .Include(s => s.Department)
                .ToListAsync();
        }
        public async Task<Subject?> GetSubjectByIdAsync(int id)
        {
            return await _context.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SubjectId == id);
        }
        public async Task AddSubjectAsync(Subject? subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            _context.Entry(subject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
    }
}