using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
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
                .ToListAsync();
        }
        public async Task<Subject?> GetSubjectByIdAsync(Guid id)
        {
            return await _context.Subjects
                .AsNoTracking()
                .Include(s => s.Lectures)
                .Include(s => s.Books)
                .FirstOrDefaultAsync(s => s.SubjectId == id);
        }
        public async Task AddSubjectAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            _context.Entry(subject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                throw new ArgumentException();
            }
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
    }
}