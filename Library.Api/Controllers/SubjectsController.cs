using Library.Domain.DTOs.Subject;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectsController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    // GET: api/Subjects
    [HttpGet]
    public async Task<IEnumerable<SubjectDto>> GetSubjects()
    {
        return await _subjectService.GetAllSubjectsAsync();
    }

    // GET: api/Subjects/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SubjectDetailsDto>> GetSubject(Guid id)
    {
        var result = await _subjectService.GetSubjectByIdAsync(id);
        return result.Match<ActionResult<SubjectDetailsDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // POST: api/Subjects
    [HttpPost]
    public async Task<ActionResult> PostSubject([FromBody] CreateSubjectDto createSubjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _subjectService.AddSubjectAsync(createSubjectDto);
        return result.Match<ActionResult>(
            id => CreatedAtAction("GetSubject", new { id }, new { id }),
            error => StatusCode(error.Code, error));
    }

    // PUT: api/Subjects/5
    [HttpPut]
    public async Task<IActionResult> PutSubject([FromBody] SubjectDto subjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _subjectService.UpdateSubjectAsync(subjectDto);
        return result.Match<IActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }

    // DELETE: api/Subjects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(Guid id)
    {
        var result = await _subjectService.DeleteSubjectAsync(id);
        return result.Match<ActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }
}