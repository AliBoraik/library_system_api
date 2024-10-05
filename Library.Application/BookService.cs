using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Book;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class BookService(
    IBookRepository bookRepository,
    ISubjectRepository subjectRepository,
    IMapper mapper,
    IUploadsService uploadsService) : IBookService
{
    public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
    {
        var lectures = await bookRepository.FindAllBooksAsync();
        return mapper.Map<IEnumerable<BookResponseDto>>(lectures);
    }

    public async Task<Result<BookResponseDto, Error>> GetBookByIdAsync(Guid id)
    {
        var lecture = await bookRepository.FindBookByIdAsync(id);
        if (lecture == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found lecture with id = {id}");
        return mapper.Map<BookResponseDto>(lecture);
    }

    public async Task<Result<Guid, Error>> AddBookAsync(CreateBookDto bookDto, Guid userId)
    {
        var bookExists = await bookRepository.FindBookByNameAsync(bookDto.Title, bookDto.SubjectId);
        if (bookExists != null)
            return new Error(StatusCodes.Status409Conflict, $"Book with title = {bookDto.Title} already exists");
        // check SubjectId 
        var subject = await subjectRepository.FindSubjectByIdAsync(bookDto.SubjectId);
        if (subject == null)
            return new Error(StatusCodes.Status404NotFound, $"Subject with Id = {bookDto.SubjectId} not found");
        if (subject.TeacherId != userId)
            return new Error(StatusCodes.Status403Forbidden, "You don't have access");
        // file info
        var bookId = Guid.NewGuid();
        var fullDirectoryPath = Path.Combine("Uploads", bookDto.SubjectId.ToString());
        var fullFilePath = Path.Combine(fullDirectoryPath, bookId.ToString()) +
                           Path.GetExtension(bookDto.File.FileName);
        // save in database
        var book = mapper.Map<Book>(bookDto);
        book.FilePath = fullFilePath;
        book.BookId = bookId;
        book.UploadedBy = userId;
        await bookRepository.AddBookAsync(book);
        // save in disk
        var uploadResult = await uploadsService.AddFile(fullDirectoryPath, fullFilePath, bookDto.File);
        if (uploadResult.IsOk) return book.BookId;
        await bookRepository.DeleteBookAsync(book);
        return uploadResult.Error;
    }

    public async Task<Result<Ok, Error>> DeleteBookAsync(Guid id, Guid userId)
    {
        var book = await bookRepository.FindBookByIdAsync(id);
        if (book == null) return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {id}");
        if (book.Subject.TeacherId != userId)
            return new Error(StatusCodes.Status403Forbidden, "Unauthorized to delete");
        await bookRepository.DeleteBookAsync(book);
        var uploadResult = uploadsService.DeleteFile(book.FilePath);
        if (!uploadResult.IsOk)
        {
            // TODO check if Can't delete file from disk 
        }

        return new Ok();
    }

    public async Task<Result<string, Error>> GetBookFilePathByIdAsync(Guid id)
    {
        var bookFilePath = await bookRepository.FindBookFilePathByIdAsync(id);
        if (bookFilePath == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Book with ID = {id}");
        return bookFilePath;
    }
}