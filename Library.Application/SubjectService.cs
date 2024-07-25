using AutoMapper;
using Library.Application.Exceptions;
using Library.Domain.DTOs.Subject;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Application;

public class SubjectService : ISubjectService
{
    private readonly IMapper _mapper;
    private readonly ISubjectRepository _subjectRepository;

    public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
    {
        _subjectRepository = subjectRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
    {
        var subjects = await _subjectRepository.GetAllSubjectsInfoAsync();
        return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
    }

    public async Task<SubjectDetailsDto> GetSubjectByIdAsync(Guid id)
    {
        var subject = await _subjectRepository.GetSubjectByIdAsync(id);
        if (subject == null)
            throw new NotFoundException($"Not found subject with id = {id}");
        return _mapper.Map<SubjectDetailsDto>(subject);
    }

    public async Task<Guid> AddSubjectAsync(CreateSubjectDto createSubjectDto)
    {
        var subject = _mapper.Map<Subject>(createSubjectDto);
        await _subjectRepository.AddSubjectAsync(subject);
        return subject.SubjectId;
    }

    public async Task UpdateSubjectAsync(SubjectDto subjectDto)
    {
        var subject = _mapper.Map<Subject>(subjectDto);
        await _subjectRepository.UpdateSubjectAsync(subject);
    }

    public async Task DeleteSubjectAsync(Guid id)
    {
        await _subjectRepository.DeleteSubjectAsync(id);
    }
}