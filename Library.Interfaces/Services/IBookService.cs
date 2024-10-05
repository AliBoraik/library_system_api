using Library.Domain;
using Library.Domain.DTOs.Book;

namespace Library.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
    Task<Result<BookResponseDto, Error>> GetBookByIdAsync(Guid id);
    Task<Result<Guid, Error>> AddBookAsync(CreateBookDto lectureDto, Guid userId);
    Task<Result<Ok, Error>> DeleteBookAsync(Guid id, Guid userId);
    Task<Result<string, Error>> GetBookFilePathByIdAsync(Guid id);
}