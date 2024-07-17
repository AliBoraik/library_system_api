using AutoMapper;
using Library.Domain.DTOs;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentsController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<IEnumerable<DepartmentInfoDto>> GetDepartments()
        {
            return await _departmentService.GetAllDepartmentsAsync();
        }
        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            var departmentDto = await _departmentService.GetDepartmentByIdAsync(id);
            if (departmentDto == null)
            {
                return NotFound();
            }
            return _mapper.Map<DepartmentDto>(departmentDto);
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, DepartmentDto departmentDto)
        {
            if (id != departmentDto.DepartmentId)
            {
                return BadRequest();
            }
            await _departmentService.UpdateDepartmentAsync(departmentDto);
            return NoContent();
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> PostDepartment(DepartmentDto departmentDto)
        {
            await _departmentService.AddDepartmentAsync(departmentDto);
            return CreatedAtAction("GetDepartment", new { id = departmentDto.DepartmentId }, departmentDto);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return NoContent();
        }
    }
}
