using Library.Application.CachePolicies;
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
    // GET: api/Subjects
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Subjects], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
    {
        var subjectsAsyncDto = await subjectService.GetAllSubjectsAsync();
        return Ok(subjectsAsyncDto);
    }

    // GET: api/Subjects/5
    [HttpGet("{subjectId:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Subjects], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<SubjectDetailsDto>> GetSubject(Guid subjectId)
    {
        var result = await subjectService.GetSubjectByIdAsync(subjectId);
        return result.Match<ActionResult<SubjectDetailsDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // POST: api/Subjects
    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult> PostSubject([FromBody] CreateSubjectDto createSubjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await subjectService.AddSubjectAsync(createSubjectDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        var subjectId = result.Value;
        return CreatedAtAction("GetSubject", new { subjectId }, new { subjectId });
    }

    // PUT: api/Subjects/5
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

    // DELETE: api/Subjects/5
    [HttpDelete("{subjectId:guid}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteSubject(Guid subjectId)
    {
        var result = await subjectService.DeleteSubjectAsync(subjectId);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects, CancellationToken.None);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        return Ok();
    }

    // POST: api/Subject/AddStudent
    [HttpPost("AddStudent")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> AddStudentToSubject([FromBody] AddStudentToSubjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await subjectService.AddStudentToSubjectAsync(request.StudentId, request.SubjectId);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Students, CancellationToken.None);
        return result.Match<ActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }
}