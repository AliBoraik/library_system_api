using Library.Domain.Constants;
using Library.Domain.DTOs.Subject;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController(ISubjectService subjectService, IOutputCacheStore cacheStore)  : ControllerBase
{
    // GET: api/Subjects
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Subjects])]
    public async Task<IEnumerable<SubjectDto>> GetSubjects()
    {
        return await subjectService.GetAllSubjectsAsync();
    }

    // GET: api/Subjects/5
    [HttpGet("{id:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Subjects])]
    public async Task<ActionResult<SubjectDetailsDto>> GetSubject(Guid id)
    {
        var result = await subjectService.GetSubjectByIdAsync(id);
        return result.Match<ActionResult<SubjectDetailsDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // POST: api/Subjects
    [HttpPost]
    public async Task<ActionResult> PostSubject([FromBody] CreateSubjectDto createSubjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await subjectService.AddSubjectAsync(createSubjectDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects,CancellationToken.None);
        var id = result.Value;
        return CreatedAtAction("GetSubject", new { id }, new { id });
    }

    // PUT: api/Subjects/5
    [HttpPut]
    public async Task<IActionResult> PutSubject([FromBody] SubjectDto subjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await subjectService.UpdateSubjectAsync(subjectDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects,CancellationToken.None);
        return Ok();
    }

    // DELETE: api/Subjects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(Guid id)
    {
        var result = await subjectService.DeleteSubjectAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Subjects,CancellationToken.None);
        return Ok();
    }
}