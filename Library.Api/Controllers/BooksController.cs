using System.Security.Claims;
using Library.Application.CachePolicies;
using Library.Domain.Constants;
using Library.Domain.DTOs.Book;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class BooksController(IBookService bookService, IOutputCacheStore cacheStore) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of books with caching applied.
    /// </summary>
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Books], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetBooks()
    {
        var booksAsyncDto = await bookService.GetAllBooksAsync();
        return Ok(booksAsyncDto);
    }

    /// <summary>
    /// Retrieves details of a specific book by its ID.
    /// </summary>
    [HttpGet("{bookId:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Books], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<BookResponseDto>> GetBook(Guid bookId)
    {
        var result = await bookService.GetBookByIdAsync(bookId);
        return result.Match<ActionResult<BookResponseDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    /// <summary>
    /// Creates a new book entry.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public async Task<ActionResult<BookResponseDto>> PostBook([FromForm] CreateBookDto createLectureDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        // Get the current user's ID
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var result = await bookService.AddBookAsync(createLectureDto, Guid.Parse(userId));
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Books, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        var bookId = result.Value;
        return CreatedAtAction("GetBook", new { bookId }, new { bookId });
    }

    /// <summary>
    /// Deletes a specific book by its ID.
    /// </summary>
    [HttpDelete("{bookId:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public async Task<IActionResult> DeleteBook(Guid bookId)
    {
        // Get the current user's ID
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var result = await bookService.DeleteBookAsync(bookId, Guid.Parse(userId));
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Books, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        return Ok();
    }

    /// <summary>
    /// Downloads the content of a specific book by its ID.
    /// </summary>
    [HttpGet("Download/{bookId:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Books])]
    public async Task<IActionResult> DownloadBook(Guid bookId)
    {
        var result = await bookService.GetBookFilePathByIdAsync(bookId);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        var path = result.Value;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
    }
}