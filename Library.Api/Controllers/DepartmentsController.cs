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
[Authorize]
public class DepartmentsController(IDepartmentService departmentService, IOutputCacheStore cacheStore) : ControllerBase
{
    // GET: api/Department
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Departments], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        var result = await departmentService.GetAllDepartmentsAsync();
        return result.Match<ActionResult<IEnumerable<DepartmentDto>>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // GET: api/Department/5
    [HttpGet("{departmentId:guid}")]
    [OutputCache(Tags = [OutputCacheTags.Departments], PolicyName = nameof(AuthCachePolicy))]
    public async Task<ActionResult<DepartmentDetailsDto>> GetDepartment(Guid departmentId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await departmentService.GetDepartmentByIdAsync(departmentId);
        return result.Match<ActionResult<DepartmentDetailsDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // POST: api/Department
    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult> PostDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await departmentService.AddDepartmentAsync(createDepartmentDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        var departmentId = result.Value;
        return CreatedAtAction("GetDepartment", new { departmentId }, new { departmentId });
    }

    // PUT: api/Department/5
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

    // DELETE: api/Department/5
    [HttpDelete("{departmentId:guid}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteDepartment(Guid departmentId)
    {
        var result = await departmentService.DeleteDepartmentAsync(departmentId);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        return Ok();
    }
}