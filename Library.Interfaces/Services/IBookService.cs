using Library.Domain.DTOs;
using Microsoft.AspNetCore.Http;

namespace Library.Interfaces.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<IEnumerable<BookDto>> GetBooksBySubjectIdAsync(int subjectId);
        Task AddBookAsync(BookDto bookDto, IFormFile file);
        Task UpdateBookAsync(BookDto bookDto, IFormFile? file);
        Task DeleteBookAsync(int id);
    }
}