using Library.Application.CachePolicies;
using Library.Domain.Constants;
using Library.Domain.DTOs.Users.Student;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class StudentsController(IStudentService studentService, IOutputCacheStore cacheStore) : ControllerBase
{
    /// <summary>
    /// Retrieves all students.
    /// </summary>
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Students], PolicyName = nameof(AuthCachePolicy))]
    public async Task<IActionResult> GetAllStudents()
    {
        var studentsAsyncDto = await studentService.GetAllStudentsAsync();
        return Ok(studentsAsyncDto);
    }
    /// <summary>
    /// Retrieves details of a specific student by their ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Students], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<StudentDto>> GetStudent(Guid id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await studentService.GetStudentByIdAsync(id);
        return result.Match<ActionResult<StudentDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }
    

    /// <summary>
    /// Retrieves students associated with a specific subject.
    /// </summary>
    [HttpGet("Subject/{id}")]
    [OutputCache(Tags = [OutputCacheTags.Students], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<List<StudentDto>>> GetStudentsBySubjectId(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await studentService.GetStudentsBySubjectAsync(id);
        return result.Match<ActionResult<List<StudentDto>>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }
        
    /// <summary>
    /// Deletes a specific student by their ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteStudent(Guid id)
    {
        var result = await studentService.DeleteStudentAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Students, CancellationToken.None);
        return Ok();
    }
}