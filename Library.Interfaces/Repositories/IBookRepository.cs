using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> FindAllBooksAsync();
    Task<Book?> FindBookByIdAsync(Guid id);
    Task<Book?> FindBookByNameAsync(string name, Guid subjectId);
    Task<string?> FindBookFilePathByIdAsync(Guid id);
    Task AddBookAsync(Book book);
    Task DeleteBookAsync(Book book);
}