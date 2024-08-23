using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class BookRepository(ApplicationDbContext context) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await context.Books.Include(b => b.Subject)
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await context.Books
            .Include(b => b.Subject)
            .FirstOrDefaultAsync(b => b.BookId == id);
    }

    public async Task AddBookAsync(Book? book)
    {
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        context.Entry(book).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await context.Books.FindAsync(id);
        context.Books.Remove(book);
        await context.SaveChangesAsync();
    }
}