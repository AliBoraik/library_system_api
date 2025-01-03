using Library.Application.CachePolicies;
using Library.Domain.Constants;
using Library.Domain.DTOs.Users.Teacher;
using Library.Domain.Results.Common;
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
    /// <summary>
    ///     Retrieves all teachers.
    /// </summary>
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Teachers], PolicyName = nameof(AuthCachePolicy))]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachersAsyncDto = await teacherService.GetAllTeachersAsync();
        return Ok(teachersAsyncDto);
    }

    /// <summary>
    ///     Retrieves details of a specific teacher by their ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Teachers], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<TeacherDto>> GetTeacher(Guid id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await teacherService.GetTeacherByIdAsync(id);
        return ResultHelper.HandleResult(result);
    }

    /// <summary>
    ///     Deletes a specific teacher by their ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeacher(Guid id)
    {
        var result = await teacherService.DeleteTeacherAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Teachers, CancellationToken.None);
        return Ok();
    }
}