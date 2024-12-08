using System.Security.Claims;
using Library.Application.CachePolicies;
using Library.Domain.Constants;
using Library.Domain.DTOs.Lecture;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class LecturesController(ILectureService lectureService, IOutputCacheStore cacheStore) : ControllerBase
{
    // GET: api/Lectures
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Lectures], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<IEnumerable<LectureResponseDto>>> GetLectures()
    {
        var lecturesAsyncDto = await lectureService.GetAllLecturesAsync();
        return Ok(lecturesAsyncDto);
    }

    // GET: api/Lectures/5
    [HttpGet("{lectureId:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Lectures], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<LectureResponseDto>> GetLecture(Guid lectureId)
    {
        var result = await lectureService.GetLectureByIdAsync(lectureId);
        return result.Match<ActionResult<LectureResponseDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // POST: api/Lectures
    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public async Task<ActionResult<LectureResponseDto>> PostLecture([FromForm] CreateLectureDto createLectureDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        // Get the current user's ID
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var result = await lectureService.AddLectureAsync(createLectureDto, Guid.Parse(userId));
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Lectures, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        var lectureId = result.Value;
        return CreatedAtAction("GetLecture", new { lectureId }, new { lectureId });
    }

    // DELETE: api/Lectures/5
    [HttpDelete("{lectureId:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public async Task<IActionResult> DeleteLecture(Guid lectureId)
    {
        // Get the current user's ID
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var result = await lectureService.DeleteLectureAsync(lectureId, Guid.Parse(userId));
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Lectures, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        return Ok();
    }

    // GET: api/Lectures/download/5
    [HttpGet("Download/{lectureId:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Lectures])]
    public async Task<IActionResult> DownloadLecture(Guid lectureId)
    {
        var result = await lectureService.GetLectureFilePathByIdAsync(lectureId);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        var path = result.Value;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
    }
}