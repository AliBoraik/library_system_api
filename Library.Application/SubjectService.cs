using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Subject;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class SubjectService : ISubjectService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;
    private readonly ISubjectRepository _subjectRepository;

    public SubjectService(ISubjectRepository subjectRepository, IMapper mapper,
        IDepartmentRepository departmentRepository)
    {
        _subjectRepository = subjectRepository;
        _mapper = mapper;
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
    {
        var subjects = await _subjectRepository.FindAllSubjectsInfoAsync();
        return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
    }

    public async Task<Result<SubjectDetailsDto, Error>> GetSubjectByIdAsync(Guid id)
    {
        var subject = await _subjectRepository.FindSubjectByIdAsync(id);
        if (subject == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found subject with id = {id}");
        return _mapper.Map<SubjectDetailsDto>(subject);
    }

    public async Task<Result<Guid, Error>> AddSubjectAsync(CreateSubjectDto createSubjectDto)
    {
        var department = await _departmentRepository.DepartmentExistsAsync(createSubjectDto.DepartmentId);
        if (!department)
            return new Error(StatusCodes.Status404NotFound,
                $"Not found department with id = {createSubjectDto.DepartmentId}");
        var subject = _mapper.Map<Subject>(createSubjectDto);
        await _subjectRepository.AddSubjectAsync(subject);
        return subject.SubjectId;
    }

    public async Task<Result<Ok, Error>> UpdateSubjectAsync(SubjectDto subjectDto)
    {
        var subjectExists = await _subjectRepository.SubjectExistsAsync(subjectDto.DepartmentId);
        if (!subjectExists)
            return new Error(StatusCodes.Status404NotFound, $"Not found subject with ID = {subjectDto.DepartmentId}");
        var subject = _mapper.Map<Subject>(subjectDto);
        await _subjectRepository.UpdateSubjectAsync(subject);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> DeleteSubjectAsync(Guid id)
    {
        var subject = await _subjectRepository.FindSubjectByIdAsync(id);
        if (subject == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found subject with id = {id}");
        await _subjectRepository.DeleteSubjectAsync(subject);
        return new Ok();
    }
}