using System.Security.Claims;
using Library.Application.CachePolicies;
using Library.Application.Common;
using Library.Domain.Constants;
using Library.Domain.DTOs.Subject;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class SubjectsController(ISubjectService subjectService, IOutputCacheStore cacheStore) : ControllerBase
{
    /// <summary>
    ///     Retrieves all subjects.
    /// </summary>
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Subjects], PolicyName = nameof(AuthCachePolicy))]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
    {
        var subjectsAsyncDto = await subjectService.GetAllSubjectsAsync();
        return Ok(subjectsAsyncDto);
    }

    /// <summary>
    ///     Retrieves details of a specific subject by its ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [OutputCache(Tags = [OutputCacheTags.Subjects], PolicyName = nameof(AuthUserIdCachePolicy))]
    public async Task<ActionResult<SubjectDetailsDto>> GetSubject(int id)
    {
        var isAdmin = User.IsInRole(AppRoles.Admin);
        if (isAdmin)
        {
            var adminResult = await subjectService.GetSubjectByIdAsync(id);
            return ResultHelper.HandleResult(adminResult);
        }
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest("Invalid user ID.");

        var userResult = await subjectService.GetUserSubjectByIdAsync(id, userGuid);
        return ResultHelper.HandleResult(userResult);
    }

    /// <summary>
    ///     Creates a new subject.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult> PostSubject([FromBody] CreateSubjectDto createSubjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await subjectService.AddSubjectAsync(createSubjectDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        var id = result.Value;
        return CreatedAtAction("GetSubject", new { id }, new { id });
    }

    /// <summary>
    ///     Updates an existing subject.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> PutSubject([FromBody] SubjectDto subjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await subjectService.UpdateSubjectAsync(subjectDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        return Ok();
    }

    /// <summary>
    ///     Deletes a specific subject by its ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var result = await subjectService.DeleteSubjectAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        return Ok();
    }
}