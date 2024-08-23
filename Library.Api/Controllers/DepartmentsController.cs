using Library.Domain;
using Library.Domain.DTOs.Department;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController(IDepartmentService departmentService) : ControllerBase
{
    // GET: api/Department
    [HttpGet]
    [OutputCache]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        var result = await departmentService.GetAllDepartmentsAsync();
        return result.Match<ActionResult<IEnumerable<DepartmentDto>>>(
            dto => Ok(dto),
            error => StatusCode(error.Code , error));
    }
    // GET: api/Department/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DepartmentDetailsDto>> GetDepartment(Guid id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await departmentService.GetDepartmentByIdAsync(id);
        return result.Match<ActionResult<DepartmentDetailsDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code , error));
    }

    // POST: api/Department
    [HttpPost]
    public async Task<ActionResult> PostDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await departmentService.AddDepartmentAsync(createDepartmentDto);
        return result.Match<ActionResult>(
            id => CreatedAtAction("GetDepartment", new { id }, new { id }),
            error => StatusCode(error.Code , error));
    }

    // PUT: api/Department/5
    [HttpPut]
    public async Task<IActionResult> PutDepartment([FromBody] DepartmentDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result =  await departmentService.UpdateDepartmentAsync(departmentDto);
        return result.Match<ActionResult>(
            _ => NoContent(),
            error => NotFound(new Response{Message = error.Message}));
    }

    // DELETE: api/Department/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        var result =  await departmentService.DeleteDepartmentAsync(id);
        return result.Match<ActionResult>(
            _ => NoContent(),
            error => StatusCode(error.Code , error));
    }
}