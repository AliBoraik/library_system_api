using AutoMapper;
using Library.Domain.DTOs.Users.Teacher;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Application;

public class TeacherService(ITeacherRepository teacherRepository, IMapper mapper) : ITeacherService
{
    public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
    {
        var teachers = await teacherRepository.FindTeachersAsync();
        return mapper.Map<IEnumerable<TeacherDto>>(teachers);
    }

    public async Task<Result<TeacherDto, Error>> GetTeacherByIdAsync(Guid id)
    {
        var teacher = await teacherRepository.FindTeacherByIdAsync(id);
        if (teacher == null)
            return Result<TeacherDto, Error>.Err(Errors.NotFound("teacher"));
        var dto = mapper.Map<TeacherDto>(teacher);
        return Result<TeacherDto, Error>.Ok(dto);
    }


    public async Task<Result<Ok, Error>> DeleteTeacherAsync(Guid id)
    {
        var teacherExists = await teacherRepository.FindTeacherByIdAsync(id);
        if (teacherExists == null)
            return Result<Ok, Error>.Err(Errors.NotFound("teacher"));
        await teacherRepository.DeleteTeacherAsync(teacherExists);
        return ResultHelper.Ok();
    }
}