using Library.Domain.Constants;
using Library.Domain.DTOs;
using Library.Domain.DTOs.Lecture;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LecturesController(ILectureService lectureService , IOutputCacheStore cacheStore) : ControllerBase
{
    // GET: api/Lectures
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Lectures])]
    public async Task<ActionResult<IEnumerable<LectureDto>>> GetLectures()
    {
        var lectures = await lectureService.GetAllLecturesAsync();
        return Ok(lectures);
    }

    // GET: api/Lectures/5
    [HttpGet("{id}")]
    [OutputCache(Tags = [OutputCacheTags.Lectures , OutputCacheTags.Subjects])]
    public async Task<ActionResult<LectureDto>> GetLecture(Guid id)
    {
        var result = await lectureService.GetLectureByIdAsync(id);
        return result.Match<ActionResult<LectureDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // POST: api/Lectures
    [HttpPost]
    public async Task<ActionResult<LectureDto>> PostLecture([FromForm] CreateLectureDto createLectureDto,
        FileUploadDto fileUploadDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await lectureService.AddLectureAsync(createLectureDto, fileUploadDto.File);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Lectures,CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects,CancellationToken.None);
        var id = result.Value;
        return CreatedAtAction("GetLecture", new { id }, new { id });
    }

    // DELETE: api/Lectures/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLecture(Guid id)
    {
        var result = await lectureService.DeleteLectureAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Lectures,CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects,CancellationToken.None);
        return Ok();
    }

    // GET: api/Lectures/download/5
    [HttpGet("Download/{id}")]
    [OutputCache(Tags = [OutputCacheTags.Lectures])]
    public async Task<IActionResult> DownloadLecture(Guid id)
    {
        var result = await lectureService.GetLectureFilePathByIdAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        var path = result.Value;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
    }
}