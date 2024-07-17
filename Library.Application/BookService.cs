using Library.Domain.DTOs;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class BookService : IBookService
{
    public Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BookDto?> GetBookByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BookDto>> GetBooksBySubjectIdAsync(int subjectId)
    {
        throw new NotImplementedException();
    }

    public Task AddBookAsync(BookDto bookDto, IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBookAsync(BookDto bookDto, IFormFile? file)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBookAsync(int id)
    {
        throw new NotImplementedException();
    }
}