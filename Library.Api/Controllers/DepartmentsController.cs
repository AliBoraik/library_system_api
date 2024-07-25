using Library.Domain.Constants;
using Library.Domain.DTOs.Department;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    // GET: api/Department
    [HttpGet]
    public async Task<IEnumerable<DepartmentDto>> GetDepartments()
    {
        return await _departmentService.GetAllDepartmentsAsync();
    }

    // GET: api/Department/5
    [HttpGet("{id:guid}")]
    [Authorize(Roles = AppRoles.Teacher)]
    public async Task<ActionResult<DepartmentDetailsDto>> GetDepartment(Guid id)
    {
        var departmentDto = await _departmentService.GetDepartmentByIdAsync(id);
        return Ok(departmentDto);
    }

    // POST: api/Department
    [HttpPost]
    public async Task<ActionResult> PostDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var id = await _departmentService.AddDepartmentAsync(createDepartmentDto);
        return CreatedAtAction("GetDepartment", new { id }, new { id });
    }

    // PUT: api/Department/5
    [HttpPut]
    public async Task<IActionResult> PutDepartment([FromBody] DepartmentDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _departmentService.UpdateDepartmentAsync(departmentDto);
        return NoContent();
    }

    // DELETE: api/Department/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        await _departmentService.DeleteDepartmentAsync(id);
        return NoContent();
    }
}