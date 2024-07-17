using AutoMapper;
using Library.Domain.DTOs;
using Library.Domain.Models;
using Library.Interfaces;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IMapper _mapper;

        public SubjectsController(ISubjectService subjectService, IMapper mapper)
        {
            _subjectService = subjectService;
            _mapper = mapper;
        }

        // GET: api/Subjects
        [HttpGet]
      
        public async Task<IEnumerable<SubjectInfoDto>> GetSubjects()
        {
           return await _subjectService.GetAllSubjectsAsync();
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubject(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);

            if (subject == null)
            {
                return NotFound();
            }
            return _mapper.Map<SubjectDto>(subject);
        }

        // PUT: api/Subjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(int id, SubjectDto subjectDto)
        {
            if (id != subjectDto.SubjectId)
            {
                return BadRequest();
            }
            await _subjectService.UpdateSubjectAsync(subjectDto);

            return NoContent();
        }

        // POST: api/Subjects
        [HttpPost]
        public async Task<ActionResult<SubjectInfoDto>> PostSubject(SubjectDto subjectDto)
        {
            await _subjectService.AddSubjectAsync(subjectDto);
            return Ok();
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            await _subjectService.DeleteSubjectAsync(id);

            return NoContent();
        }
    }
}