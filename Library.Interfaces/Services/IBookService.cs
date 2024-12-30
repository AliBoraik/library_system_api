using Library.Domain.DTOs.Book;
using Library.Domain.Models;
using Library.Domain.Results;
using Library.Domain.Results.Common;

namespace Library.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
    Task<Result<BookResponseDto, Error>> GetBookByIdAsync(Guid id);
    Task<Result<Guid, Error>> AddBookAsync(CreateBookDto lectureDto, Guid userId);
    Task<Result<Ok, Error>> DeleteBookAsync(Guid id, Guid userId);
    Task<Result<Book, Error>> GetBookFilePathByIdAsync(Guid userId, Guid bookId);
    Task<Result<Book, Error>> HasAccessToBook(Guid userId, Guid bookId);
}