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
        return await _subjectService.GetSubjectByIdAsync(id);
    }

    // POST: api/Subjects
    [HttpPost]
    public async Task<ActionResult> PostSubject([FromBody] CreateSubjectDto createSubjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var id = await _subjectService.AddSubjectAsync(createSubjectDto);
        return CreatedAtAction("GetSubject", new { id }, new { id });
    }

    // PUT: api/Subjects/5
    [HttpPut]
    public async Task<IActionResult> PutSubject(SubjectDto subjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _subjectService.UpdateSubjectAsync(subjectDto);

        return NoContent();
    }

    // DELETE: api/Subjects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(Guid id)
    {
        await _subjectService.DeleteSubjectAsync(id);

        return NoContent();
    }
}