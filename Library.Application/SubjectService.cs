using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Subject;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class SubjectService(
    ISubjectRepository subjectRepository,
    IMapper mapper,
    IDepartmentRepository departmentRepository)
    : ISubjectService
{
    public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
    {
        var subjects = await subjectRepository.FindAllSubjectsInfoAsync();
        return mapper.Map<IEnumerable<SubjectDto>>(subjects);
    }

    public async Task<Result<SubjectDetailsDto, Error>> GetSubjectByIdAsync(Guid id)
    {
        var subject = await subjectRepository.FindSubjectDetailsByIdAsync(id);
        if (subject == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found subject with id = {id}");
        return mapper.Map<SubjectDetailsDto>(subject);
    }

    public async Task<Result<Guid, Error>> AddSubjectAsync(CreateSubjectDto createSubjectDto)
    {
        var department = await departmentRepository.DepartmentExistsAsync(createSubjectDto.DepartmentId);
        if (!department)
            return new Error(StatusCodes.Status404NotFound,
                $"Not found department with id = {createSubjectDto.DepartmentId}");
        var subject = mapper.Map<Subject>(createSubjectDto);
        await subjectRepository.AddSubjectAsync(subject);
        return subject.Id;
    }

    public async Task<Result<Ok, Error>> AddStudentToSubjectAsync(Guid studentId, Guid subjectId)
    {
        var subject = await subjectRepository.FindSubjectDetailsByIdAsync(subjectId);
        if (subject == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found subject with id = {subjectId}");
        await subjectRepository.AddStudentToSubjectAsync(studentId, subjectId);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> UpdateSubjectAsync(SubjectDto subjectDto)
    {
        var subjectExists = await subjectRepository.SubjectExistsAsync(subjectDto.DepartmentId);
        if (!subjectExists)
            return new Error(StatusCodes.Status404NotFound, $"Not found subject with ID = {subjectDto.DepartmentId}");
        var subject = mapper.Map<Subject>(subjectDto);
        await subjectRepository.UpdateSubjectAsync(subject);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> DeleteSubjectAsync(Guid id)
    {
        var subject = await subjectRepository.FindSubjectByIdAsync(id);
        if (subject == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found subject with id = {id}");
        await subjectRepository.DeleteSubjectAsync(subject);
        return new Ok();
    }
}