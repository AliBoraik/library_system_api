using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Users.Teacher;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

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
            return new Error(StatusCodes.Status404NotFound, $"Not found teacher with id = {id}");
        return mapper.Map<TeacherDto>(teacher);
    }


    public async Task<Result<Ok, Error>> DeleteTeacherAsync(Guid id)
    {
        var teacherExists = await teacherRepository.FindTeacherByIdAsync(id);
        if (teacherExists == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Teacher with ID = {id}");
        await teacherRepository.DeleteTeacherAsync(teacherExists);
        return new Ok();
    }
}