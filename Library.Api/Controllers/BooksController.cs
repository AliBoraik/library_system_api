using Library.Domain.Constants;
using Library.Domain.DTOs;
using Library.Domain.DTOs.Book;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookService bookService, IOutputCacheStore cacheStore) : ControllerBase
{
    // GET: api/Books
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Books])]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var lectures = await bookService.GetAllBooksAsync();
        return Ok(lectures);
    }
    
    // GET: api/Books/5
    [HttpGet("{id}")]
    [OutputCache(Tags = [OutputCacheTags.Books])]
    public async Task<ActionResult<BookDto>> GetBook(Guid id)
    {
        var result = await bookService.GetBookByIdAsync(id);
        return result.Match<ActionResult<BookDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }
    // POST: api/Books
    [HttpPost]
    public async Task<ActionResult<BookDto>> PostGetBook([FromForm] CreateBookDto createLectureDto,
        FileUploadDto fileUploadDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await bookService.AddBookAsync(createLectureDto, fileUploadDto.File);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Books,CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects,CancellationToken.None);
        var id = result.Value;
        return CreatedAtAction("GetBook", new { id }, new { id });
    }
    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var result = await bookService.DeleteBookAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Books, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects,CancellationToken.None);
        return Ok();
    }

    // GET: api/Books/download/5
    [HttpGet("download/{id}")]
    [OutputCache(Tags = [OutputCacheTags.Books])]
    public async Task<IActionResult> DownloadBook(Guid id)
    {
        var result = await bookService.GetBookFilePathByIdAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        var path = result.Value;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
    }
    
}