using System.Security.Claims;
using Library.Application.CachePolicies;
using Library.Domain.Constants;
using Library.Domain.DTOs.Lecture;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class LecturesController(ILectureService lectureService, IOutputCacheStore cacheStore) : ControllerBase
{
    /// <summary>
    ///     Retrieves all lectures.
    /// </summary>
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Lectures], PolicyName = nameof(AuthCachePolicy))]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<IEnumerable<LectureResponseDto>>> GetLectures()
    {
        var lecturesAsyncDto = await lectureService.GetAllLecturesAsync();
        return Ok(lecturesAsyncDto);
    }

    /// <summary>
    ///     Retrieves details of a specific lecture by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Lectures], PolicyName = nameof(AuthCachePolicy))]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<LectureResponseDto>> GetLecture(Guid id)
    {
        var result = await lectureService.GetLectureByIdAsync(id);
        return ResultHelper.HandleResult(result);
    }

    /// <summary>
    ///     Creates a new lecture.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public async Task<ActionResult<LectureResponseDto>> PostLecture([FromForm] CreateLectureDto createLectureDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));
        var result = await lectureService.AddLectureAsync(createLectureDto, userGuid);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Lectures, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        var id = result.Value;
        return CreatedAtAction("GetLecture", new { id }, new { id });
    }

    /// <summary>
    ///     Deletes a specific lecture by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public async Task<IActionResult> DeleteLecture(Guid id)
    {
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));
        var result = await lectureService.DeleteLectureAsync(id, userGuid);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Lectures, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        return Ok();
    }

    /// <summary>
    ///     Downloads the content of a specific lecture by its ID.
    /// </summary>
    [HttpGet("Download/{id:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Lectures], PolicyName = nameof(AuthUserIdCachePolicy))]
    public async Task<IActionResult> DownloadLecture(Guid id)
    {
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));
        var result = await lectureService.GetLectureFilePathByIdAsync(userGuid, id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        var lecture = result.Value;
        var path = lecture.FilePath;
        var fileName = lecture.Title;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", fileName + Path.GetExtension(path));
    }
}