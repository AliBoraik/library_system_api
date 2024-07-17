using Library.Domain.Models;

namespace Library.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task AddBookAsync(Book? book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
}