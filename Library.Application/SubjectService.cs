using AutoMapper;
using Library.Domain.DTOs;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Application
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectInfoDto>> GetAllSubjectsAsync()
        {
            var subjects = await _subjectRepository.GetAllSubjectsAsync();
            return _mapper.Map<IEnumerable<SubjectInfoDto>>(subjects);
        }

        public async Task<SubjectDto?> GetSubjectByIdAsync(int id)
        {
            var subject = await _subjectRepository.GetSubjectByIdAsync(id);
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task AddSubjectAsync(SubjectDto subjectDto)
        {
            var subject = _mapper.Map<Subject>(subjectDto);
            await _subjectRepository.AddSubjectAsync(subject);
        }

        public async Task UpdateSubjectAsync(SubjectDto subjectDto)
        {
            var subject = _mapper.Map<Subject>(subjectDto);
            await _subjectRepository.UpdateSubjectAsync(subject);
        }

        public async Task DeleteSubjectAsync(int id)
        {
            await _subjectRepository.DeleteSubjectAsync(id);
        }
    }
}