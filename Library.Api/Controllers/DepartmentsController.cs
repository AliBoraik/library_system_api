using Library.Domain.DTOs.Department;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
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
        public async Task<IEnumerable<DepartmentInfoDto>> GetDepartments()
        {
            return await _departmentService.GetAllDepartmentsAsync();
        }
        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(Guid id)
        {
            var departmentDto = await _departmentService.GetDepartmentByIdAsync(id);
            if (departmentDto == null)
            {
                throw new KeyNotFoundException($"Not found department with id = {id}");
            }
            return Ok(departmentDto);
        }

        // PUT: api/Department/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(Guid id, DepartmentDto departmentDto)
        {
            if (id != departmentDto.DepartmentId)
            {
                return BadRequest();
            }
            await _departmentService.UpdateDepartmentAsync(departmentDto);
            return NoContent();
        }

        // POST: api/Department
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> PostDepartment(DepartmentDto departmentDto)
        {
            await _departmentService.AddDepartmentAsync(departmentDto);
            return CreatedAtAction("GetDepartment", new { id = departmentDto.DepartmentId }, departmentDto);
        }

        // DELETE: api/Department/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return NoContent();
        }
    }
}
