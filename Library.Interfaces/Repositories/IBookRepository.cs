using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> FindAllBooksAsync();
    Task<Book?> FindBookWithSubjectByIdAsync(Guid id);
    Task<Book?> FindBookByNameAsync(string name, int subjectId);
    Task<string?> FindBookFilePathByIdAsync(Guid id);
    Task AddBookAsync(Book book);
    Task DeleteBookAsync(Book book);
}