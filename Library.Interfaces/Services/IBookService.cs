using Library.Domain;
using Library.Domain.DTOs.Book;
using Microsoft.AspNetCore.Http;

namespace Library.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<Result<BookDto, Error>> GetBookByIdAsync(Guid id);
    Task<Result<Guid, Error>> AddBookAsync(CreateBookDto lectureDto, IFormFile file);
    Task<Result<string, Error>> GetBookFilePathByIdAsync(Guid id);
    Task<Result<Ok, Error>> DeleteBookAsync(Guid id);
}