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
    [OutputCache(Duration = 10)]
    public async Task<IEnumerable<DepartmentDto>> GetDepartments()
    {
        return await departmentService.GetAllDepartmentsAsync();
    }

    // GET: api/Department/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DepartmentDetailsDto>> GetDepartment(Guid id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var departmentDto = await departmentService.GetDepartmentByIdAsync(id);
        return Ok(departmentDto);
    }

    // POST: api/Department
    [HttpPost]
    public async Task<ActionResult> PostDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var id = await departmentService.AddDepartmentAsync(createDepartmentDto);
        return CreatedAtAction("GetDepartment", new { id }, new { id });
    }

    // PUT: api/Department/5
    [HttpPut]
    public async Task<IActionResult> PutDepartment([FromBody] DepartmentDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await departmentService.UpdateDepartmentAsync(departmentDto);
        return NoContent();
    }

    // DELETE: api/Department/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        await departmentService.DeleteDepartmentAsync(id);
        return NoContent();
    }
}