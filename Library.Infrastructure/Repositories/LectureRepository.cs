using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
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

        public async Task<Lecture?> GetLectureByIdAsync(int id)
        {
            return await _context.Lectures
                .AsNoTracking()
                .Include(l => l.Subject)
                .FirstOrDefaultAsync(l => l.LectureId == id);
        }

        public async Task AddLectureAsync(Lecture? lecture)
        {
            await _context.Lectures.AddAsync(lecture);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLectureAsync(Lecture lecture)
        {
            _context.Entry(lecture).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLectureAsync(int id)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync();
        }
    }
}