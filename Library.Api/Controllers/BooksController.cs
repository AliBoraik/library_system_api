using Library.Application.CachePolicies;
using Library.Domain.Constants;
using Library.Domain.DTOs.Book;
using Library.Domain.Extensions;
using Library.Domain.Results.Common;
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
    ///     Retrieves a list of books with caching applied.
    /// </summary>
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Books], PolicyName = nameof(AuthCachePolicy))]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetBooks()
    {
        var booksAsyncDto = await bookService.GetAllBooksAsync();
        return Ok(booksAsyncDto);
    }

    /// <summary>
    ///     Retrieves details of a specific book by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Books], PolicyName = nameof(AuthCachePolicy))]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<BookResponseDto>> GetBook(Guid id)
    {
        var result = await bookService.GetBookByIdAsync(id);
        return ResultHelper.HandleResult(result);
    }

    /// <summary>
    ///     Creates a new book entry.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public async Task<ActionResult<BookResponseDto>> PostBook([FromForm] CreateBookDto createLectureDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        // Extract userId from JWT token
        var userId = User.GetUserId();
        var result = await bookService.AddBookAsync(createLectureDto, userId);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Books, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        var id = result.Value;
        return CreatedAtAction("GetBook", new { id }, new { id });
    }

    /// <summary>
    ///     Deletes a specific book by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        // Extract userId from JWT token
        var userId = User.GetUserId();
        var result = await bookService.DeleteBookAsync(id, userId);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Books, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        return Ok();
    }

    /// <summary>
    ///     Downloads the content of a specific book by its ID.
    /// </summary>
    [HttpGet("Download/{id:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Books], PolicyName = nameof(AuthUserIdCachePolicy))]
    public async Task<IActionResult> DownloadBook(Guid id)
    {
        // Extract userId from JWT token
        var userId = User.GetUserId();
        var result = await bookService.GetBookFilePathByIdAsync(userId, id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        var book = result.Value;
        var path = book.FilePath;
        var fileName = book.Title;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", fileName + Path.GetExtension(path));
    }
}