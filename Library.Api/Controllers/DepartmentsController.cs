using Library.Domain.DTOs.Department;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

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
    [OutputCache]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        var result = await _departmentService.GetAllDepartmentsAsync();
        return result.Match<ActionResult<IEnumerable<DepartmentDto>>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // GET: api/Department/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DepartmentDetailsDto>> GetDepartment(Guid id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _departmentService.GetDepartmentByIdAsync(id);
        return result.Match<ActionResult<DepartmentDetailsDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // POST: api/Department
    [HttpPost]
    public async Task<ActionResult> PostDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _departmentService.AddDepartmentAsync(createDepartmentDto);
        return result.Match<ActionResult>(
            id => CreatedAtAction("GetDepartment", new { id }, new { id }),
            error => StatusCode(error.Code, error));
    }

    // PUT: api/Department/5
    [HttpPut]
    public async Task<IActionResult> PutDepartment([FromBody] DepartmentDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _departmentService.UpdateDepartmentAsync(departmentDto);
        return result.Match<ActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }

    // DELETE: api/Department/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        var result = await _departmentService.DeleteDepartmentAsync(id);
        return result.Match<ActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }
}