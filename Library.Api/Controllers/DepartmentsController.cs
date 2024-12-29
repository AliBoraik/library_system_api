using System.Security.Claims;
using Library.Application.CachePolicies;
using Library.Application.Common;
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
    ///     Retrieves all departments.
    /// </summary>
    [HttpGet]
    [OutputCache(Tags = [OutputCacheTags.Departments], PolicyName = nameof(AuthUserIdCachePolicy))]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        var isAdmin = User.IsInRole(AppRoles.Admin);
        if (isAdmin)
        {
            var adminResult = await departmentService.GetAllDepartmentsAsync();
            return ResultHelper.HandleResult(adminResult);
        }
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest("Invalid user ID.");
        var userResult = await departmentService.GetAllUserDepartmentsAsync(userGuid);
        return ResultHelper.HandleResult(userResult);
    }
    /// <summary>
    ///     Retrieves details of a specific department by its ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [OutputCache(Tags = [OutputCacheTags.Departments], PolicyName = nameof(AuthUserIdCachePolicy))]
    public async Task<ActionResult<DepartmentDetailsDto>> GetDepartment(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var isAdmin = User.IsInRole(AppRoles.Admin);
        if (isAdmin)
        {
            var adminResult = await departmentService.GetDepartmentByIdAsync(id);
            return ResultHelper.HandleResult(adminResult);
        }
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest("Invalid user ID.");
        var userResult = await departmentService.GetUserDepartmentByIdAsync(userGuid , id);
        return ResultHelper.HandleResult(userResult);
      
    }

    /// <summary>
    ///     Creates a new department.
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
    ///     Updates an existing department.
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
    ///     Deletes a specific department by its ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var result = await departmentService.DeleteDepartmentAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Departments, CancellationToken.None);
        return Ok();
    }
}