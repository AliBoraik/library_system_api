using Library.Application.CachePolicies;
using Library.Domain.Constants;
using Library.Domain.DTOs.Users.Teacher;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class TeachersController(ITeacherService teacherService, IOutputCacheStore cacheStore) : ControllerBase
{
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Teachers], PolicyName = nameof(AuthCachePolicy))]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await teacherService.GetAllTeachersAsync();
        return Ok(teachers);
    }

    [HttpGet("{teacherId:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Teachers], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<TeacherDto>> GetTeacher(Guid teacherId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await teacherService.GetTeacherByIdAsync(teacherId);
        return result.Match<ActionResult<TeacherDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    [HttpDelete("{teacherId:guid}")]
    public async Task<IActionResult> DeleteTeacher(Guid teacherId)
    {
        var result = await teacherService.DeleteTeacherAsync(teacherId);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Teachers, CancellationToken.None);
        return Ok();
    }
}