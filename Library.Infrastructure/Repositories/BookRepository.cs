using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class BookRepository(AppDbContext context) : IBookRepository
{
    public async Task<IEnumerable<Book>> FindAllBooksAsync()
    {
        return await context.Books
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> FindBookByIdAsync(Guid id)
    {
        return await context.Books
            .Where(b => b.Id == id)
            .Include(b => b.Subject)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<Book?> FindBookByNameAsync(string name, int subjectId)
    {
        return await context.Books
            .AsNoTracking()
            .Where(l => l.Title == name && l.SubjectId == subjectId)
            .FirstOrDefaultAsync();
    }

    public async Task<string?> FindBookFilePathByIdAsync(Guid id)
    {
        return await context.Books
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => l.FilePath)
            .FirstOrDefaultAsync();
    }

    public async Task AddBookAsync(Book book)
    {
        await context.Books.AddAsync(book);
        await Save();
    }

    public async Task DeleteBookAsync(Book book)
    {
        context.Books.Remove(book);
        await Save();
    }

    private async Task Save()
    {
        await context.SaveChangesAsync();
    }
}