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
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Students], PolicyName = nameof(AuthCachePolicy))]
    public async Task<IActionResult> GetAllStudents()
    {
        var studentsAsyncDto = await studentService.GetAllStudentsAsync();
        return Ok(studentsAsyncDto);
    }

    [HttpGet("{studentId:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Students], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<StudentDto>> GetStudent(Guid studentId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await studentService.GetStudentByIdAsync(studentId);
        return result.Match<ActionResult<StudentDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }


    [HttpDelete("{studentId:guid}")]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        var result = await studentService.DeleteStudentAsync(studentId);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Students, CancellationToken.None);
        return Ok();
    }

    // GET api/students/subject/{subjectId}
    [HttpGet("Subject/{subjectId}")]
    [OutputCache(Tags = [OutputCacheTags.Students], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<List<StudentDto>>> GetStudentsBySubjectId(Guid subjectId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await studentService.GetStudentsBySubjectAsync(subjectId);
        return result.Match<ActionResult<List<StudentDto>>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }
}