using AutoMapper;
using Library.Domain.Constants;
using Library.Domain.DTOs.Book;
using Library.Domain.DTOs.Notification;
using Library.Domain.Models;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Library.Application;

public class BookService(
    IBookRepository bookRepository,
    ISubjectRepository subjectRepository,
    IProducerService producerService,
    UserManager<User> userManager,
    IStudentRepository studentRepository,
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
        var book = await bookRepository.FindBookWithSubjectByIdAsync(id);
        if (book == null)
            return Result<BookResponseDto, Error>.Err(Errors.NotFound("book"));
        var dto = mapper.Map<BookResponseDto>(book);
        return Result<BookResponseDto, Error>.Ok(dto);
    }

    public async Task<Result<Guid, Error>> AddBookAsync(CreateBookDto bookDto, Guid userId)
    {
        var bookExists = await bookRepository.FindBookByNameAsync(bookDto.Title, bookDto.SubjectId);
        if (bookExists != null)
            return Result<Guid, Error>.Err(Errors.Conflict("title already exists"));
        // check bookDto.SubjectId 
        var subject = await subjectRepository.FindSubjectByIdAsync(bookDto.SubjectId);
        if (subject == null)
            return Result<Guid, Error>.Err(Errors.NotFound("subject"));
        // check subject.TeacherId 
        if (subject.TeacherId != userId)
            // Check if userId is in the Admin role
            if (!await userManager.IsInRoleAsync(new User { Id = userId }, AppRoles.Admin))
                return Result<Guid, Error>.Err(Errors.Forbidden(" add book"));

        // file info
        var bookId = Guid.NewGuid();
        // save in disk
        var uploadResult = await uploadsService.AddFile(bookDto.SubjectId.ToString(), bookId.ToString(), bookDto.File);
        if (!uploadResult.IsOk)
            return uploadResult.Error;
        // save in database
        var book = mapper.Map<Book>(bookDto);
        book.FilePath = uploadResult.Value;
        book.Id = bookId;
        book.UploadedBy = userId;
        await bookRepository.AddBookAsync(book);

        // Run sending notification  in the background
        var recipients = await studentRepository.FindStudentIdsByDepartmentIdAsync(subject.DepartmentId);

        _ = Task.Run(async () =>
        {
            // Send notification in the background
            await SendBulkNotificationAsync($"Book Added - {bookDto.Title}",
                "New Book Added you can download or read it", recipients, subject.TeacherId);
        });

        return Result<Guid, Error>.Ok(book.Id);
    }

    public async Task<Result<Ok, Error>> DeleteBookAsync(Guid id, Guid userId)
    {
        var book = await bookRepository.FindBookWithSubjectByIdAsync(id);

        if (book == null)
            return Result<Ok, Error>.Err(Errors.NotFound("lecture"));
        if (book.Subject.TeacherId != userId)
            return Result<Ok, Error>.Err(Errors.Forbidden("delete book"));
        await bookRepository.DeleteBookAsync(book);
        var uploadResult = uploadsService.DeleteFile(book.FilePath);
        if (!uploadResult.IsOk)
        {
            // TODO check if Can't delete file from disk 
        }

        return ResultHelper.Ok();
    }

    public async Task<Result<Book, Error>> GetBookFilePathByIdAsync(Guid userId, Guid bookId)
    {
        var accessToBook = await HasAccessToBook(userId, bookId);
        if (!accessToBook.IsOk)
            return accessToBook.Error;
        var book = accessToBook.Value;
        return book;
    }

    public async Task<Result<Book, Error>> HasAccessToBook(Guid userId, Guid bookId)
    {
        var book = await bookRepository.FindBookWithSubjectByIdAsync(bookId);
        if (book == null)
            return Result<Book, Error>.Err(Errors.NotFound("book"));
        // teacher can access books 
        if (book.Subject.TeacherId == userId)
            return book;
        // Get the current user
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<Book, Error>.Err(Errors.Unauthorized("access book"));
        // Admins can download any book
        if (await userManager.IsInRoleAsync(user, AppRoles.Admin))
            return book;
        // Students can access books linked to their department
        if (user.DepartmentId != book.Subject.DepartmentId)
            return Result<Book, Error>.Err(Errors.Forbidden("access book"));
        return Result<Book, Error>.Ok(book);
    }


    private async Task SendBulkNotificationAsync(string title, string message, IEnumerable<Guid> recipients,
        Guid senderId)
    {
        // Create the notification request
        var notificationRequest = new CreateBulkNotificationDto
        {
            Title = title,
            Message = message,
            SenderId = senderId,
            RecipientUserIds = recipients
        };

        // Send the notification event asynchronously
        await producerService.SendBulkNotificationEventToAsync(AppTopics.NotificationTopic, notificationRequest);
    }
}