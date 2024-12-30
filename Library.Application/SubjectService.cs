using AutoMapper;
using Library.Domain.DTOs.Subject;
using Library.Domain.Models;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

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

    public async Task<Result<SubjectDetailsDto, Error>> GetUserSubjectByIdAsync(int subjectId, Guid userId)
    {
        var departments = (await departmentRepository.FindAllUserDepartmentsAsync(userId)).ToList();
        if (!departments.Any())
            return Result<SubjectDetailsDto, Error>.Err(Errors.NotFound("subject"));
        var subjectResult = await GetSubjectByIdAsync(subjectId);
        if (!subjectResult.IsOk)
            return subjectResult.Error;
        var subjectDto = subjectResult.Value;
        if (departments.Any(d => d.Id == subjectDto.DepartmentId))
            return Result<SubjectDetailsDto, Error>.Ok(subjectDto);
        return Result<SubjectDetailsDto, Error>.Err(Errors.Forbidden("get subject"));
    }

    public async Task<Result<SubjectDetailsDto, Error>> GetSubjectByIdAsync(int subjectId)
    {
        var subject = await subjectRepository.FindSubjectDetailsByIdAsync(subjectId);
        if (subject == null)
            return Result<SubjectDetailsDto, Error>.Err(Errors.NotFound("subject"));

        var dto = mapper.Map<SubjectDetailsDto>(subject);
        return Result<SubjectDetailsDto, Error>.Ok(dto);
    }

    public async Task<Result<int, Error>> AddSubjectAsync(CreateSubjectDto createSubjectDto)
    {
        var department = await departmentRepository.DepartmentExistsAsync(createSubjectDto.DepartmentId);
        if (!department)
            return Result<int, Error>.Err(Errors.NotFound("department"));
        var subject = mapper.Map<Subject>(createSubjectDto);
        await subjectRepository.AddSubjectAsync(subject);
        return Result<int, Error>.Ok(subject.Id);
    }

    public async Task<Result<Ok, Error>> UpdateSubjectAsync(SubjectDto subjectDto)
    {
        var subjectExists = await subjectRepository.SubjectExistsAsync(subjectDto.DepartmentId);
        if (!subjectExists)
            return Result<Ok, Error>.Err(Errors.NotFound("subject"));
        var subject = mapper.Map<Subject>(subjectDto);
        await subjectRepository.UpdateSubjectAsync(subject);
        return ResultHelper.Ok();
    }

    public async Task<Result<Ok, Error>> DeleteSubjectAsync(int id)
    {
        var subject = await subjectRepository.FindSubjectByIdAsync(id);
        if (subject == null)
            return Result<Ok, Error>.Err(Errors.NotFound("subject"));
        await subjectRepository.DeleteSubjectAsync(subject);
        return ResultHelper.Ok();
    }
}