using Library.Application.CachePolicies;
using Library.Domain.Constants;
using Library.Domain.DTOs.Department;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class DepartmentsController(IDepartmentService departmentService, IOutputCacheStore cacheStore) : ControllerBase
{
    /// <summary>
    /// Retrieves all departments.
    /// </summary>
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Departments], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        var result = await departmentService.GetAllDepartmentsAsync();
        return result.Match<ActionResult<IEnumerable<DepartmentDto>>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }
    
    /// <summary>
    /// Retrieves details of a specific department by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Departments], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<DepartmentDetailsDto>> GetDepartment(Guid id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await departmentService.GetDepartmentByIdAsync(id);
        return result.Match<ActionResult<DepartmentDetailsDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    /// <summary>
    /// Creates a new department.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult> PostDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await departmentService.AddDepartmentAsync(createDepartmentDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        var id = result.Value;
        return CreatedAtAction("GetDepartment", new { id }, new { id });
    }

    /// <summary>
    /// Updates an existing department.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> PutDepartment([FromBody] DepartmentDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await departmentService.UpdateDepartmentAsync(departmentDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        return Ok();
    }

    /// <summary>
    /// Deletes a specific department by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        var result = await departmentService.DeleteDepartmentAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        return Ok();
    }
}