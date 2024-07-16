using AutoMapper;
using Library.Domain.DTOs;
using Library.Domain.Models;
using Library.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectsController(ISubjectRepository subjectService, IMapper mapper)
        {
            _subjectRepository = subjectService;
            _mapper = mapper;
        }

        // GET: api/Subjects
        [HttpGet]
      
        public async Task<IEnumerable<SubjectInfoDto>> GetSubjects()
        {
            var subjects = await _subjectRepository.GetAllSubjectsAsync();
            return _mapper.Map<IEnumerable<SubjectInfoDto>>(subjects);
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubject(int id)
        {
            var subject = await _subjectRepository.GetSubjectByIdAsync(id);

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
            var subject =  _mapper.Map<Subject>(subjectDto);
            await _subjectRepository.UpdateSubjectAsync(subject);

            return NoContent();
        }

        // POST: api/Subjects
        [HttpPost]
        public async Task<ActionResult<SubjectDto>> PostSubject(SubjectDto subjectDto)
        {
            var subject = _mapper.Map<Subject>(subjectDto);
            await _subjectRepository.AddSubjectAsync(subject);
            var createdSubjectDto = _mapper.Map<SubjectDto>(subject);
            return CreatedAtAction("GetSubject", new { id = createdSubjectDto.SubjectId }, createdSubjectDto);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            await _subjectRepository.DeleteSubjectAsync(id);

            return NoContent();
        }
    }
}